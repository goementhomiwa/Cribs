namespace Cribs.Web.DataContexts.CribsMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CribImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                        Cover = c.Boolean(nullable: false),
                        RentCrib_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentCribs", t => t.RentCrib_Id)
                .Index(t => t.RentCrib_Id);
            
            CreateTable(
                "dbo.RentCribs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        MonthlyPrice = c.Double(nullable: false),
                        NumberOfRooms = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Location = c.String(nullable: false, maxLength: 100),
                        Available = c.DateTime(nullable: false),
                        DatePost = c.DateTime(),
                        DateExpire = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CribImages", "RentCrib_Id", "dbo.RentCribs");
            DropIndex("dbo.CribImages", new[] { "RentCrib_Id" });
            DropTable("dbo.RentCribs");
            DropTable("dbo.CribImages");
        }
    }
}
