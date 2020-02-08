namespace Nihility.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategories", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ProductCategories", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ProductCategories", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.ProductCategories", "ModifiedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ProductCategories", "MetaKeywork", c => c.String(maxLength: 256));
            AddColumn("dbo.ProductCategories", "MetaDescription", c => c.String(maxLength: 256));
            AddColumn("dbo.ProductCategories", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCategories", "Status");
            DropColumn("dbo.ProductCategories", "MetaDescription");
            DropColumn("dbo.ProductCategories", "MetaKeywork");
            DropColumn("dbo.ProductCategories", "ModifiedBy");
            DropColumn("dbo.ProductCategories", "ModifiedDate");
            DropColumn("dbo.ProductCategories", "CreatedBy");
            DropColumn("dbo.ProductCategories", "CreatedDate");
        }
    }
}
