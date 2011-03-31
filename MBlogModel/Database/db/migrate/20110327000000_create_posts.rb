
class CreatePosts < ActiveRecord::Migration

  def self.up
    create_table :posts do |t|
      t.column :title, :string, :null => false
      t.column :blogPost, :text, :null => false
      t.column :posted, :datetime, :null => false
      t.column :edited, :datetime, :null => true
    end    
  end

  
  def self.down
    
    drop_table :posts
  
  end
end

