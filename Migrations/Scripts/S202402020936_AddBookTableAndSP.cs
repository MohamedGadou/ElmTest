using Dapper;
using DbUp.Engine;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Migrations.Scripts
{
    public class S202402020936_AddBookTableAndSP : IScript
    {
        public string ProvideScript(Func<IDbCommand> commandFactory)
        {

            using var command = (commandFactory() as SqlCommand)!;
            const string createBookViewCommand = @"
                    CREATE TABLE [dbo].[BookView](
                    	[Id] [bigint] IDENTITY(1,1) NOT NULL,
                    	[Title] [nvarchar](60) NOT NULL,
                    	[Description] [nvarchar](300) NOT NULL,
                    	[Author] [nvarchar](40) NOT NULL,
                    	[Cover] [nvarchar](max) NULL,
                    	[PublishedDate] [nvarchar](100) NOT NULL,
                    	[LastModified] [datetime2](7) NOT NULL,
                     CONSTRAINT [PK_Dumb] PRIMARY KEY CLUSTERED 
                    (
                    	[Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    ";

            command.Connection.Execute(createBookViewCommand);

            const string titleIndex = @"
                    CREATE NONCLUSTERED INDEX [NonClusteredIndex-20240202-214212] ON [dbo].[BookView]
                    (
                    	[Title] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ";
            command.Connection.Execute(titleIndex);

            const string descriptionIndex = @"
                    CREATE NONCLUSTERED INDEX [NonClusteredIndex-20240202-214440] ON [dbo].[BookView]
                    (
                    	[Description] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ";
            command.Connection.Execute(descriptionIndex);


            const string authorIndex = @"
                    CREATE NONCLUSTERED INDEX [NonClusteredIndex-20240202-214626] ON [dbo].[BookView]
                    (
                    	[Author] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ";
            command.Connection.Execute(authorIndex);

            const string publishedDateIndex = @"
                    CREATE NONCLUSTERED INDEX [NonClusteredIndex-20240202-214833] ON [dbo].[BookView]
                    (
                    	[PublishedDate] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ";
            command.Connection.Execute(publishedDateIndex);

            const string searchBooksSP = @"
                CREATE PROCEDURE [dbo].[Books_GetAll]  
                  @rowOffset int = 0,
                  @fetchNextRows int = 100,
                  @searchKey nvarchar = null
                AS
                BEGIN   
                    SELECT * 
                   	FROM dbo.BookView bv
                   	WHERE (
                   	@searchKey is null OR 
                   	(bv.Title LIKE '% '+@searchKey +'%') OR
                   	(bv.Author LIKE '% '+@searchKey +'%') OR
                   	(bv.Description LIKE '% '+@searchKey +'%') OR
                   	(bv.PublishedDate LIKE '% '+@searchKey +'%')
                   	)
                   	ORDER BY Id desc
                    OFFSET @rowOffset ROWS FETCH NEXT @fetchNextRows ROWS ONLY;
                END
                ";
            command.Connection.Execute(searchBooksSP);

            const string insertIntoBookViewCommand = @"
                        INSERT INTO [dbo].[BookView]
                                                   ([Title]
                                                   ,[Description]
                                                   ,[Author]
                                                   ,[PublishedDate]
                                                   ,[Cover]
                                                   ,[LastModified])
                        select  
                            info.Title as Title, 
                            info.Description as Description, 
                            info.Author as Author, 
                            info.PublishDate as PublishedDate, 
                            info.CoverBase64 as Cover, 
                            b.LastModified as LastModified from 
                        dbo.Book b 
                        CROSS APPLY 
                        OPENJSON(b.BookInfo) WITH 
                        (
                        	Title varchar(max)  '$.BookTitle',
                        	Description varchar(max) '$.BookDescription',
                        	Author varchar(max) '$.Author',
                        	PublishDate varchar(max) '$.PublishDate',
                        	CoverBase64 varchar(max) '$.CoverBase64'
                        ) as info
                        ";
            command.Connection.Execute(insertIntoBookViewCommand,commandTimeout: int.MaxValue);

            return "";
        }

    }
}
