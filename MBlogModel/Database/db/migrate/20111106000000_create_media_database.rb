class CreateMediaDatabase < ActiveRecord::Migration
  def self.up
    create_table :media do |t|
      t.string  :title,        :null => false
      t.string  :file_name,    :null => false
      t.string  :caption
      t.string  :description
      t.string  :alternate
      t.integer :year,   :null => false
      t.integer :month,  :null => false
      t.integer :day,  :null => false
      t.string  :mime_type,   :null => false
      t.integer :alignment,   :null => false
      t.integer  :size,        :null => false
      t.integer :user_id,     :null => false
      t.binary  :medium,  :limit => 10000000, :null => false
    end    

    execute "ALTER TABLE [dbo].[media]  WITH CHECK ADD  CONSTRAINT [FK_media_users] FOREIGN KEY([user_id]) REFERENCES [dbo].[users] ([id])"
    execute "ALTER TABLE [dbo].[media] CHECK CONSTRAINT [FK_media_users]"
  end

  
  def self.down    
    execute "ALTER TABLE [dbo].[media] DROP CONSTRAINT [FK_media_users]"
    drop_table :media
  end
end

