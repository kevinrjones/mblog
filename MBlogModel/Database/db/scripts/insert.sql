INSERT INTO [$(DATABASE)].[dbo].[users]
		([name],[email],[hashed_password],[salt], [is_site_admin])
		VALUES
		('Kevin Jones', 'kevin@requiredattribute.com', 'foo' ,'bar', 'true')

INSERT INTO [$(DATABASE)].[dbo].[blogs]
		([title],[description],[nickname])
		VALUES
		('Wish I Knew Now', 'The blog of this site', 'kevin' )

INSERT INTO [$(DATABASE)].[dbo].[users_blogs]
		([user_id],[blog_id])
		VALUES
		(1, 1)


INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Why this Blog?'
   ,'Several things came together at the same time. I wanted to start blogging again. My current blog engine is written in Java (Blojsom) and though very good, I don''t really do Java anymore so wanted to move off it. I did a presentation at <a href="www.devweek.com">DevWeek</a> last week and it seem to go down pretty well.  I need to spend more time with MVC.    <p>Given all that it seemed to make sense for me to put that together into a simple blog engine. And it is simple   <p>Multi user: not yet - but I will design it that way</p>    <p>Fancy editors: not yet, but they will be planned in</p>    <p>Extensible: let''s not get too far ahead of ourselves</p>    <p>Javascript/Ajax: yep, this is part of the plan</p>    <p>Wiki markup, plain text markup: all part of the future</p></p>    <p>   I''m going to use MVC (3 as of the writing) and EF 4.1. I''m going to start code first, not because I think we should    be building databases this way but becasue I havn''t used it before.    I want to use one of the NoSQL databases, because a) I''ve never used them and b) it seems like a natural fit    for a document centric system (we''ll see)    I want to use the publish and deploy tools in VS 2010, partly to see if they are any good and also because I haven''t    used them before (I can see a theme here)    I''m probably going to put this up on github and open source it, not because I think other people will use it but yes,     and because I haven''t done that before!    </p>'
  ,'2011-03-27 12:00:00', 1)


INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Up and running'
   ,'<p>So the first aim is to get this up and running on a local instance of SQL Server If you can see this then that must have worked!</p><p>This (I hope) will have been deployed with a deploy script and when I know how to do that I''ll write about it here!</p>'
  ,'2011-03-27 13:00:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Up and running (well almost)'
   ,'<p>I''m adding a ''code'' widget to the site to format code appropriately. Now, I don''t even have a layout/CSS started yet <img src="images/InitialBlog.png" style="width: 100%"></src> but as this is going to be mostly about coding then layout of code is important. I''m going to use ''Syntax Highlighter'' !!! which is on http://alexgorbatchev.com/SyntaxHighlighter/</p>'
  ,'2011-03-27 14:00:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Isn''t this exciting'
   ,'<p>So, the first real post about the site (with screenshot!) I''m using code-first initially, although I know I''ll be moving away from that. This means I need models to create the database tables and to map to those tables, I''ll have more to say about models and view models soon. My first model is a <code>Post</code> that looks like this</p><pre  class="brush: csharp">
    public class Post
    {    
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(int.MaxValue)]
        public string BlogPost { get; set; }
        [Required]
        public DateTime Posted { get; set; }
        public DateTime? Edited { get; set; }
    }</pre>
<p>What happens when entity framework tries to create a database from this is (roughly) this. It takes the Id property as the primary key (convention over configuration) All the other fields it also uses convention for, which I override with the attributes. So the reference types get created as nullable (the <pre>[Required]</pre> makes them non nullable. The strings get given a max length of 128, the <pre>[MaxLength]</pre> attribute overrides that. Making the Edited property a <pre>Nullable<DateTime></pre> makes that nullable. And off we go!</p>'
  ,'2011-03-27 15:00:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Walking Skeleton'
   ,'<p>So the ''walking skeleton'' is in place. What is a ''Walking Skeleton'', it''s the bare bones of an application that you need in place to help you move forward. If I was writing this using a pure TDD then I would have started with a test project, written a test for a <span class="inline-code">Post</span> class, then written the class (even though this class is anaemic). Then added a test project for my controller, written some tests and then started the controller. But come on, this is boring. Not just that, it''s counter productive. As developers we want to see forward progress, hence the walking skeleton. There''s a great description <a href="http://alistair.cockburn.us/Walking+skeleton">here</a>.</p><p>Next step is testing and thinking about multi-user before I add an editor.</p>'
  ,'2011-03-30 21:32:00', 1)


INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Migrations'
   ,'<p>
So as soon as I started to use Entity Framework code first to manage the database I knew it wasn''t going to work. Having said that I still need a way to manage the creation and updating of the database. This is going to be very fluid initially as a) I''m still using SQL Server and b) things change a lot during the initial phases of the app. To manage the migrations of the SQL I''m using Runy Active::Record migrations, and to manage the data I''m using a sql script.
</p>
<p>
A migration looks like this:
<pre class="brush: ruby">
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
</pre>
with an up and a down method. I have a rake task:
<pre class="brush: ruby">
require ''active_record''
require ''yaml''
require ''logger''

task :default => :migrate

desc "Migrate the database through scripts in db/migrate. Target specific version with VERSION=x"
task :migrate => :environment do
  ActiveRecord::Migrator.migrate(''db/migrate'', ENV["VERSION"] ? ENV["VERSION"].to_i : nil )
end

task :environment do
  @config = YAML::load(File.open(''database.yml''))
  @logger = File.join(File.dirname(__FILE__), ''database.log'')

  ActiveSupport::LogSubscriber.colorize_logging=false
  @environment = ENV["RAILS_ENV"] || "development"
  ActiveRecord::Base.establish_connection(@config[@environment])
  ActiveRecord::Base.logger = Logger.new(File.open(@logger, ''a''))
end
</pre>
that runs the migrations, this is similar to the Rails Rake script but simpler.
</p>
<p>
I still have issues with data, I need to be able to backup and restore the database across versions and across environments. I have a staging environment and I will need to bring data back from my production environment to there. That needs to be added to the todo list.
</p>'
  ,'2011-03-31 07:44:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('To Do list?'
   ,'<ul>
   <li>Issue tracking tool</li>
   <li>Continuous integration</li>
   <li>Spell checker!</li>
   <li>Investigate NoSQL</li>
   <li>User management on the site</li>
   <li>Per-blog theming</li>
   <li>database management and migrations over and above AvtiveRecord</li>
   </ul>'
  ,'2011-03-31 07:53:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Gotta Love JetBrains'
   ,'<p>I''ve just installed dotCover, TeamCity and YouTrack and it was trivially easy. Have CI builds running off GitHub and the output looks like this <img src=''images/teamcity.png'' style="width:100%"/></p>'
  ,'2011-03-31 18:47:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('First steps to a multi user blog?'
   ,'<p>I''ve just started the process of adding multi-user support to this blog. I want to have urls like http://www.requiredattribute.com/kevin where kevin is the blog nickname. To get to this I''ve added several tables (at the time of posting this is only on my dev box, not on the staging or live servers). To do this I''ve added a couple of tables and new model classes.
</p>
<p>
The new migration looks like this - I''ve munged everything into one migration as this stuff is too intimately related to be separated:
<pre class="brush: ruby">
class CreateInitialDatabase &lt; ActiveRecord::Migration

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
      t.string :text, :null => false
      t.integer :user_id, :null => false
      t.integer :post_id, :null => false
      t.string :email, :null => false
      t.datetime :commented, :null => true
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
</pre>
This may yet change but it''s an initial stab. Notice that I''m using ''_'' in the column names. I chose to do this for a couple of reasons, 1) it''s the Rails way and I may yet a Rails version of this, and 2) as this is a learning exercise I wanted to see how code-first entity framework would handle this.
</p>
<p>
[Some of] the corresponding classes for the above tables look like this
<pre class="brush :csharp">
public class Blog
{
    public virtual int Id { get; private set; }
    [Required]
    public virtual string Title { get; private set; }
    [Required]
    public virtual string Description { get; private set; }
    [Required]
    public virtual string Nickname { get; private set; }

    public virtual ICollection&lt;Post> Posts { get; set; }

    [ForeignKey("user_id")]
    public virtual ICollection&lt;User> Users { get; set; }
}
</pre>
<pre class="brush :csharp">
public class Post
{
    public int Id { get; set; }
    [Required]
    public string Title { get; private set; }
    [Required]
    [MaxLength(int.MaxValue)]
    public string BlogPost { get; private set; }
    [Required]
    public DateTime Posted { get; private set; }
    public DateTime? Edited { get; private set; }

    public virtual Blog Blog{ get; set; }
}
</pre>
So Blog has a 1:n relationship to Post (notice there''s no foreign key relation ship defined in the SQL, again this is the Rails way!) and to use this I want code that said, get all the blog posts for a blog whose nickname is ''foo''
 That required a couple of more steps.
</p>
<p>
The first thing that needed doing was the routes needed changing.
<pre class="brush :csharp">
public static void RegisterRoutes(RouteCollection routes)
{
    // ...
    routes.MapRoute(
        "Posts-Default",
        "{nickname}/{action}",
        new { controller = "Post", action = "Index" }
    );

    // ...
}
</pre>
This says that the first paramter after the root is the nickname, there''s a discussion to be had here for the default value for the ''nickname''. The way it will work currently is that if an arbitrary nickname is entered then there is no error, just an empty list of posts to be returned.
</p>
<p>
This nickname is passed on to the controller, so:
<pre class="brush :csharp">
public ActionResult Index(string nickname)
{
    IList&lt;Post> blogs = _blogPostRepository.GetBlogPosts(nickname);
</pre>
The nickname is passed as the first parameter to the Index method and is used to get the posts for this blog 
</p>
<p>
The final step then is to update the repository to only get the posts for this blog.
<pre class="brush :csharp">
public IList&lt;Post> GetBlogPosts(string nickname)
{
    return (from f in Entities
        orderby f.Posted descending 
        where f.Blog.Nickname == nickname
        select f)
        .ToList();                
}
</pre>
This is a trivial change, I''m using the Post''s, Blog relationship to get only Posts for this Blog. This works, but the first thing I wondered was what the SQL looked like. Running the SQL Server Profiler you can see the session and the generated SQL
<pre class="brush :sql">
exec sp_executesql N''SELECT 
[Project1].[Id] AS [Id], 
[Project1].[Title] AS [Title], 
[Project1].[BlogPost] AS [BlogPost], 
[Project1].[Posted] AS [Posted], 
[Project1].[Edited] AS [Edited], 
[Project1].[Blog_Id] AS [Blog_Id]
FROM ( SELECT 
	[Extent1].[Id] AS [Id], 
	[Extent1].[Title] AS [Title], 
	[Extent1].[BlogPost] AS [BlogPost], 
	[Extent1].[Posted] AS [Posted], 
	[Extent1].[Edited] AS [Edited], 
	[Extent1].[Blog_Id] AS [Blog_Id]
	FROM  [dbo].[Posts] AS [Extent1]
	INNER JOIN [dbo].[Blogs] AS [Extent2] ON [Extent1].[Blog_Id] = [Extent2].[Id]
	WHERE [Extent2].[Nickname] = @p__linq__0
)  AS [Project1]
ORDER BY [Project1].[Posted] DESC'',N''@p__linq__0 nvarchar(4000)'',@p__linq__0=N''kevin''
</pre>
Which is more or less what you would expect!'
  ,'2011-04-03 20:10:00', 1)

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted]
		,[blog_id])
     VALUES
  ('Use of the repository pattern?'
   ,'<p>
Writing data access code involves choices, which database, do I use an ORM?, do I use data access patterns?, if so, which one(s)? For this blog I''m currently using SQL Server, although as I''ve said I''d like to try out a NoSQL database. I''m also using Entity Framework (currently CTP 5 of EF code first). As for data access patterns ''repository'' seems to be the flavour of the month and that''s what I''m using. Using repository allows me to separate the use of the data access layer from the implementation of the access. In this case I have EF as the implementation but the users of the implmentation, the controllers simply code against interfaces.
</p>
<p>
The patterns is based around a single interface and implementation and then specific interfaces for the repositories that I am using (see <a href=''http://rocksolidknowledge.com/ScreenCasts.mvc''>Andy''s screencast for a great overview</a>. The basic interface looks like this:
<pre class="brush: csharp">
public interface IRepository&lt;T> : IDisposable
{
    IQueryable&lt;T> Entities { get; }
    T New();
    void Add(T entity);
    void Create(T entity);
    void Delete(T entity);
    void Save();
}
</pre>
This generic interface gives all repositories those methods, in a given implementation you can use those methods directly, or more likely provide your own API over the top of this abstraction. For example I have an IBlogPostRepository that looks like this:
<pre class="brush: csharp">
public interface IBlogPostRepository : IRepository&lt;Post>
{
    Post GetBlogPost(int id);
    IList&lt;Post> GetBlogPosts(string nickname);
}
</pre>
This lets users of the interface use specific methods
</p>
<p>
As to implementation, again there is a base class
<pre class="brush: csharp">
public abstract class BaseEfRepository&lt;T> : IRepository&lt;T> where T : class
{
    private readonly IDbSet&lt;T> _dbSet;
    private readonly DbContext _dataDbContext;

    protected BaseEfRepository(DbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
        _dbSet = dataDbContext.Set&lt;T>();
    }

    public IQueryable&lt;T> Entities
    {
        get { return _dbSet; }
    }

    public T New()
    {
        return _dbSet.Create();
    }

    public void Add(T entity)
    {
        _dbSet.Attach(entity);
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Save()
    {
        _dataDbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dataDbContext.Dispose();
    }
}
</pre>
This is Entity Framework specific, notice it is using DbContext and DbSet which are part of EF code first rather than the moer common ObjectContext and ObjectSet. Each repository has it''s own DbCOntext implementation that provides the layer between the model and the database. The BlogPost DbContext looks like this:
<pre class="brush: csharp">
public class BlogPostDbContext : DbContext
{
    public BlogPostDbContext(string connectionString)
        : base(connectionString){}

    public DbSet&lt;Post> Posts { get; set; } 
}
</pre>
This simply takes a connection string and gets the base class to make a connection to the database. It also contains a DbSet which by convention maps to that table in the database. This class is used in the base repository implementation. For example that the Entities method uses the DbSet to get its data.
</p>
<p>
Finally there''s the repository implementation for each entity, the BlogPost implementation looks like this:
<pre class="brush: csharp">
public class BlogPostPostRepository : BaseEfRepository&lt;Post>, IBlogPostRepository
{
    public BlogPostPostRepository(string connectionString)
        : base(new BlogPostDbContext(connectionString))
    {
    }

    public Post GetBlogPost(int id)
    {
        var b = (from e in Entities
                    where e.Id == id
                    select e).FirstOrDefault();
        return b;
    }

    public IList&lt;Post> GetBlogPosts(string nickname)
    {

        if (string.IsNullOrEmpty(nickname))
            return (from f in Entities
                    orderby f.Posted descending
                    select f)
                .ToList();

        return (from f in Entities
                orderby f.Posted descending
                where f.Blog.Nickname == nickname
                select f)
            .ToList();
    }
}
</pre>
This relies on the underlying abstraction being able to return something that I can run LINQ queries against but does not rely on EF at all.
</p>
<p>
I use these interfaces in the controllers. These are injected using an IoC container, more on that later.
</p>
'
  ,'2011-04-26 20:02:00', 1)
