class CreateImagesDatabase < ActiveRecord::Migration
  def self.up
    create_table :images do |t|
      t.string  :name,        :null => false
      t.string  :mime_type
      t.string  :width,       :null => false
      t.integer :user_id,     :null => false
    end    
  end

  
  def self.down    
    drop_table :images
  end
end

