class NicknameBlacklist < ActiveRecord::Base

end

class UsernameBlacklist < ActiveRecord::Base

end

class CreateBlacklistDatabase < ActiveRecord::Migration

  def self.up
    create_table :nickname_blacklists do |t|
      t.string :name, :null => false
    end    

    create_table :username_blacklists do |t|
      t.string :name, :null => false
    end    

    blacklist = %w{about account activate add admin administrator api app
                    apps archive archives auth better blog cache cancel 
                    careers cart changelog checkout codereview compare config 
                    configuration connect contact create delete direct_messages
                    documentation download downloads edit email employment enterprise
                    facebook faq favorites feed feedback feeds fleet fleets follow
                    followers following friend friends group groups gist help
                    home hosting hostmaster idea  ideas index info invitations
                    invite is it json job jobs lists login logout logs
                    mail map maps mine mis news oauth oauth_clients offers
                    openid order orders organizations plans popular privacy
                    projects put post recruitment register remove replies
                    root rss sales save search security sessions settings
                    shop signup sitemap ssl ssladmin ssladministrator
                    sslwebmaster status stories styleguide subscribe
                    subscriptions support sysadmin sysadministrator terms tour
                    translations trends twitter twittr update unfollow
                    unsubscribe url user user weather widget widgets wiki
                    ww www wwww xfn xml xmpp yml yaml}
    
    blacklist.each do |word|
        NicknameBlacklist.create([{:name => word}])
    end

    blacklist = %w{admin administrator hostmaster root ssladmin
                    sysadmin webmaster info is it mis ssladministrator
                    sslwebmaster postmaster}
    
    blacklist.each do |word|
        UsernameBlacklist.create([{:name => word}])
    end
    
  end

  
  def self.down    
    drop_table :nickname_blacklists  
    drop_table :username_blacklists  
  end
end

