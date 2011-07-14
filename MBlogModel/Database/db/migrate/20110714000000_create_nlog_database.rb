#CREATE TABLE [dbo].[NLog_Error](
#	    [Id] [int] IDENTITY(1,1) NOT NULL,
#	    [time_stamp] [datetime] NOT NULL,
#	    [host] [nvarchar](max) NOT NULL,
#	    [type] [nvarchar](50) NOT NULL,
#	    [message] [nvarchar](max) NOT NULL,
#	    [level] [nvarchar](50) NOT NULL,
#	    [logger] [nvarchar](50) NOT NULL,
#	    [stacktrace] [nvarchar](max) NOT NULL,

class CreateNlogDatabase < ActiveRecord::Migration
  def self.up
    create_table :nlog do |t|
      t.datetime    :time_stamp,    :null => true
      t.text        :host,          :null => true, :limit => 60
      t.string      :type,          :null => true, :limit => 50
      t.text        :message,       :null => true
      t.text        :source,        :null => true
      t.string      :level,         :null => true, :limit => 50
      t.string      :logger,        :null => true, :limit => 50
      t.text        :stacktrace,    :null => true, :limit => 50
    end        
  end

  
  def self.down    
    drop_table :nlog
  end
end

