
class AddTotalPostsToBlog < ActiveRecord::Migration

  def self.up
    add_column :blogs, :total_posts, :integer, :null => false, :default => 0
  end

  
  def self.down    
    remove_column :blogs, :total_posts
  end

end

