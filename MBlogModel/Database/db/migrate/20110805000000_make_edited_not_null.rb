class MakeEditedNotNull < ActiveRecord::Migration
  def self.up
    execute "update posts set edited = posted where edited is null"
    change_column :posts, :edited, :datetime, :null => false       
    add_column :blogs, :last_updated, :datetime, :null => false, :default => Time.now    
  end

  
  def self.down    
    remove_column :blogs, :last_updated
    change_column :posts, :edited, :datetime, :null => true
  end
end

