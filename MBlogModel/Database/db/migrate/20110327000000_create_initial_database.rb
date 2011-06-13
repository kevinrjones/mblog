
class CreateInitialDatabase < ActiveRecord::Migration

  def self.up
    create_table :users do |t|
      t.string :name,            :null => false
      t.string :email,           :null => false
      t.string :hashed_password, :null => false
      t.string :salt,            :null => false
      t.boolean :is_site_admin,  :null => false
    end    
    
    create_table :blogs do |t|
        t.string :title,        :null => false
        t.string :description,  :null => false
        t.string :nickname,     :null => false
        t.integer :user_id,     :null => false
        t.boolean :comments_enabled, :null => false, :default => true
        t.boolean :comment_approval, :null => false, :default => false
    end

    create_table :users_blogs do |t|
        t.integer :blog_id, :null => false
        t.integer :user_id, :null => false
    end    

    create_table :posts do |t|
      t.string :title, :null => false
      t.text :blogPost, :null => false
      t.datetime :posted, :null => false
      t.datetime :edited, :null => true
      t.integer :blog_id, :null => false
    end    

    create_table :comments do |t|
      t.text :text, :null => false
      t.integer :post_id, :null => false
      t.integer :user_id, :null => true
      t.string :name, :null => true
      t.string :email, :null => true
      t.datetime :commented, :null => false
    end    
    
  end

  
  def self.down    
    drop_table :users  
    drop_table :blogs  
    drop_table :users_blogs  
    drop_table :posts  
    drop_table :comments  
  end
end

