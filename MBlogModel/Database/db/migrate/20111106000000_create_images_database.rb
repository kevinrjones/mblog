class CreateImagesDatabase < ActiveRecord::Migration
  def self.up
    create_table :images do |t|
      t.string  :title,        :null => false
      t.string  :file_name,    :null => false
      t.string  :caption
      t.string  :description
      t.string  :alternate
      t.integer :year,   :null => false
      t.integer :month,  :null => false
      t.integer :day,  :null => false
      t.string  :mime_type,   :null => false
      t.string  :alignment,   :null => false
      t.integer  :size,        :null => false
      t.integer :user_id,     :null => false
      t.binary  :image,  :limit => 10000000, :null => false
    end    

    execute "ALTER TABLE [dbo].[images]  WITH CHECK ADD  CONSTRAINT [FK_images_users] FOREIGN KEY([user_id]) REFERENCES [dbo].[users] ([id])"
    execute "ALTER TABLE [dbo].[images] CHECK CONSTRAINT [FK_images_users]"
  end

  
  def self.down    
    execute "ALTER TABLE [dbo].[images] DROP CONSTRAINT [FK_images_users]"
    drop_table :images
  end
end

