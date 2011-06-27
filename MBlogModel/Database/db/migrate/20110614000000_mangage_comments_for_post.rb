
class MangageCommentsForPost < ActiveRecord::Migration

  def self.up
    add_column :posts, :comments_enabled, :boolean, :null => false, :default => true    
  end

  
  def self.down    
    remove_column :posts, :comments_enabled
  end

end

