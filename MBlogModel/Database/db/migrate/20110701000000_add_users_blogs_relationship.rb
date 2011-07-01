
class AddUsersBlogsRelationship < ActiveRecord::Migration

  def self.up
    execute "ALTER TABLE [dbo].[blogs] DROP CONSTRAINT [FK_blogs_users]"

    remove_column :blogs, :user_id

    execute "ALTER TABLE [dbo].[users_blogs]  WITH CHECK ADD  CONSTRAINT [FK_users_blogs_blogs] FOREIGN KEY([blog_id]) REFERENCES [dbo].[blogs] ([id])"
    execute "ALTER TABLE [dbo].[users_blogs] CHECK CONSTRAINT [FK_users_blogs_blogs]"

    execute "ALTER TABLE [dbo].[users_blogs]  WITH CHECK ADD  CONSTRAINT [FK_users_blogs_users] FOREIGN KEY([user_id]) REFERENCES [dbo].[users] ([id])"
    execute "ALTER TABLE [dbo].[users_blogs] CHECK CONSTRAINT [FK_users_blogs_users]"

  end

  
  def self.down    
    execute "ALTER TABLE [dbo].[users_blogs] DROP CONSTRAINT [FK_users_blogs_blogs]"
    execute "ALTER TABLE [dbo].[users_blogs] DROP CONSTRAINT [FK_users_blogs_users]"

    add_column :blogs, :user_id, :integer, :null => false, :default => 1    

    execute "ALTER TABLE [dbo].[blogs]  WITH CHECK ADD  CONSTRAINT [FK_blogs_users] FOREIGN KEY([user_id]) REFERENCES [dbo].[users] ([id])"
    execute "ALTER TABLE [dbo].[blogs] CHECK CONSTRAINT [FK_blogs_users]"
  end

end

