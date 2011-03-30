INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted])
     VALUES
  ('Why this Blog?'
   ,'Several things came together at the same time. I wanted to start blogging again. My current blog engine is written in Java (Blojsom) and though very good, I don''t really do Java anymore so wanted to move off it. I did a presentation at <a href="www.devweek.com">DevWeek</a> last week and it seem to go down pretty well.  I need to spend more time with MVC.    <p>Given all that it seemed to make sense for me to put that together into a simple blog engine. And it is simple   <p>Multi user: not yet - but I will design it that way</p>    <p>Fancy editors: not yet, but they will be planned in</p>    <p>Extensible: let''s not get too far ahead of ourselves</p>    <p>Javascript/Ajax: yep, this is part of the plan</p>    <p>Wiki markup, plain text markup: all part of the future</p></p>    <p>   I''m going to use MVC (3 as of the writing) and EF 4.1. I''m going to start code first, not because I think we should    be building databases this way but becasue I havn''t used it before.    I want to use one of the NoSQL databases, because a) I''ve never used them and b) it seems like a natural fit    for a document centric system (we''ll see)    I want to use the publish and deploy tools in VS 2010, partly to see if they are any good and also because I haven''t    used them before (I can see a theme here)    I''m probably going to put this up on github and open source it, not because I think other people will use it but yes,     and because I haven''t done that before!    </p>'
  ,'2011-03-27 12:00:00')


INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted])
     VALUES
  ('Up and running'
   ,'<p>So the first aim is to get this up and running on a local instance of SQL Server If you can see this then that must have worked!</p><p>This (I hope) will have been deployed with a deploy script and when I know how to do that I''ll write about it here!</p>'
  ,'2011-03-27 13:00:00')

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted])
     VALUES
  ('Up and running (well almost)'
   ,'<p>I''m adding a ''code'' widget to the site to format code appropriately. Now, I don''t even have a layout/CSS started yet (I need to take screenshots, but to do that I have to store images, well for now they can go onto disk!) but as this is going to be mostly about coding then layout of code is important. I''m going to use ''Syntax Highlighter'' !!! which is on http://alexgorbatchev.com/SyntaxHighlighter/</p>'
  ,'2011-03-27 14:00:00')

INSERT INTO [$(DATABASE)].[dbo].[posts]
		([title]
		,[blogPost]
		,[posted])
     VALUES
  ('Isn''t this exciting'
   ,'<p>So, the first real post about the site (with screenshot!) I''m using code-first initially, although I know I''ll be moving away from that. This means I need models to create the database tables and to map to those tables, I''ll have more to say about models and view models soon. My first model is a <code>Post</code> that looks like this</p><pre  class="brush: csharp">    public class Post    {        public int Id { get; set; }        [Required]        public string Title { get; set; }        [Required]        [MaxLength(int.MaxValue)]        public string BlogPost { get; set; }        [Required]        public DateTime Posted { get; set; }        public DateTime? Edited { get; set; }    }</pre><p>What happens when entity framework tries to create a database from this is (roughly) this. It takes the Id property as the primary key (convention over configuration) All the other fields it also uses convention for, which I override with the attributes. So the reference types get created as nullable (the <pre>[Required]</pre> makes them non nullable. The strings get given a max length of 128, the <pre>[MaxLength]</pre> attribute overrides that. Making the Edited property a <pre>Nullable<DateTime></pre> makes that nullable. And off we go!</p>'
  ,'2011-03-27 15:00:00')

GO

