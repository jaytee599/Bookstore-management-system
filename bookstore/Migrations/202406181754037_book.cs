namespace bookstore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class book : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(nullable: false),
                        Biography = c.String(),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ISBN = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BorrowingDate = c.DateTime(),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Rentals",
                c => new
                    {
                        RentalId = c.Int(nullable: false, identity: true),
                        ISBN = c.String(),
                        RentalDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        RentalAvailable = c.Boolean(nullable: false),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RentalId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rentals", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Rentals", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "AuthorId" });
            DropTable("dbo.Rentals");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
