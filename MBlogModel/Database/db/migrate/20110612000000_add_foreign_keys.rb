
class AddForeignKeys < ActiveRecord::Migration

  def self.up
    execute "ALTER TABLE [dbo].[blogs]  WITH CHECK ADD  CONSTRAINT [FK_blogs_users] FOREIGN KEY([user_id]) REFERENCES [dbo].[users] ([id])"
    execute "ALTER TABLE [dbo].[blogs] CHECK CONSTRAINT [FK_blogs_users]"

    execute "ALTER TABLE [dbo].[comments]  WITH CHECK ADD  CONSTRAINT [FK_comments_posts] FOREIGN KEY([post_id]) REFERENCES [dbo].[posts] ([id])"
    execute "ALTER TABLE [dbo].[comments] CHECK CONSTRAINT [FK_comments_posts]"

    execute "ALTER TABLE [dbo].[comments]  WITH CHECK ADD  CONSTRAINT [FK_comments_users] FOREIGN KEY([user_id]) REFERENCES [dbo].[users] ([id])"
    execute "ALTER TABLE [dbo].[comments] CHECK CONSTRAINT [FK_comments_users]"

    execute "ALTER TABLE [dbo].[posts]  WITH CHECK ADD  CONSTRAINT [FK_posts_blogs] FOREIGN KEY([blog_id]) REFERENCES [dbo].[blogs] ([id])"
    execute "ALTER TABLE [dbo].[posts] CHECK CONSTRAINT [FK_posts_blogs]"

  end

  
  def self.down    
    execute "ALTER TABLE [dbo].[blogs] DROP CONSTRAINT [FK_blogs_users]"
    execute "ALTER TABLE [dbo].[comments] DROP CONSTRAINT [FK_comments_posts]"
    execute "ALTER TABLE [dbo].[comments] DROP CONSTRAINT [FK_comments_users]"
    execute "ALTER TABLE [dbo].[posts] DROP CONSTRAINT [FK_posts_blogs]"
  end

end

