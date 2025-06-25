
MegaBook Read Me
Please see the MegaBooksDoc.pdf for complete docs for the system. Or right click the inspector and select help.

Note:
When importing the package into Unity 4.x you might see some error messages regarding Mesh Serialization, this is a bug in Unity when it converts projects from Unity 3.x please see the thread at http://forum.unity3d.com/threads/210417-project-4-2-to-4-3-Mismatched-serialization-in-the-builtin-class-Mesh

Introduction
If you have ever needed an animated book in your game then MegaBook is what you need, it is a complete procedural book builder and animator with an advanced page turning animation system and content builder.

MegaBook allows you to quickly and easily create multiple page books with page turning animation in Unity. You can have any number of pages and have the system generate the page meshes for you or you can provide your own custom page meshes or objects and the system will animate those and create a complete book for you. You have complete control over the textures used for the pages and there are options to allow you to have background textures and content textures merged with a mask option. You also have complete control over the generated page meshes with options to control the size of the page and how many vertices are used to build it. Also included is a system to swap pages out for ones with holes in so for big books overdraw is greatly reduced.

The page turning animation can also be controlled so you can define how much a page turns, where it starts to turn etc, MegaBook has dozens of parametes you can tweak to change how the book works all of which can be changed in Editor mode or at runtime if you need. You can also attach objects to each page so it is easy to add GUI elements or even 3d objects to each page and the book system will manage those for you, turning them with the page and disabling them when they cant be seen.

There is an API of methods you can use to easily control the book such as NextPage, PrevPage, SetPage also methods to replace content textures per page or even download new textures direct to a page from the internet or from files.

Creating a Book
In MegaBook 2 we have made it easier to create a new book. Go to the Windows menu and click 'Create a Book' this will open a window. In the library drop down select a style of book you would like to make and then in the covers dropdown a cover type then click 'Create Book' that's it, a new book will be added to you scene.

If you are using URP or HDRP then you should change the 'Shader Texture Name Override' to change the shader param MegaBook uses to set the textures. MegaBook will pick the name _BaseMap if it detects a pipeline installed but you may need to change that depending on the shaders you are using eg _BaseColorMap 

If you drag the Page slider you will see the pages turn. The content and style of the book will be set by the Book Style you selected, you can change anything you need easily in the inspector.

The first thing to do is set the number of pages you want in your book so to do this change the Num Pages option until you have the desired page count. Next we need to set the size of the book by opening the Mesh Params section in the inspector. Here you can change the width and length of the pages as well as set the number of vertices to use for each page. When you have set the size and adjusted the number of vertices then you should open the Pages foldout in the inspector. This is where we actually add the content for the book. There will be one page already added for you. It is important to note that if the actual number of content pages does not equal the actual number of physical pages in the book then the book will repeat page content until the book is filled. To change the content for the front and back of the page you just need to select the textures you want to use in the front and back texture boxes. You should see the page contents update as you select your textures.

When you have selected your textures you can add further pages to the book by clicking the 'Add Page' button. When you have more than one content page in your book you can choose which one you are currently editing by changing the 'Edit Page' slider. You will see the inspector change to show the values for the page you select.

Once you have finished adding your content then your book is ready to use. To control which page is displayed you just need to change the 'Flip' float value in the MegaBookBuilder component. The book component has some API methods you can call to control the book such as NextPage, PrevPage, SetPage etc. See below for definitions of the methods. Also included in the system is an example BookControl script which shows how you can control your newly created book.

Once you have a working book then you can play with the values in the Page Turn Options to see how they effect the page turn animation. See below for a description of all the params in the system and what they do.

The Demo scene does not include the Ships, Chest, Gun models or the music, all but the guns are free assets and the links are below.

Assets used in the demo but not included in the asset
Fluyt Ship Model	- https://retrostylegames.com/portfolio/free-3d-model-of-pirate-ship-fluyt/
Free Pirate Music	- https://assetstore.unity.com/packages/audio/music/pirate-music-pack-232467?aid=1101lGoK
Free Pirate Ship	- https://assetstore.unity.com/packages/3d/vehicles/sea/stylized-pirate-ship-200192?aid=1101lGoK
Amada Chest Model	- https://sketchfab.com/3d-models/armada-chest-82202f320d954ca5a2c59ca05a71c21d
Cannon Model		- https://assetstore.unity.com/packages/3d/props/weapons/free-cannon-pack-148796?aid=1101lGoK
Flintlock Pistols	- https://assetstore.unity.com/packages/3d/props/guns/night-quartet-flintlock-pistol-77566?aid=1101lGoK

v1.17
MegaBook 2 made comaptible with Unity 6

v1.16
Objects attached to pages will now correctly move with the book.

v1.15
Added a MapPoint method to MegaBook builder and MegaBookPage. You can use this to find a position for a point on page. Vector3 p = book.MapPoint(pagenum, pos); Will return the position on the page in local space of the book. There is also a MapPoint(pagenum, u, v) where us anv and values 0 to 1, so normalized position on the page.

v1.14
MegaBook now defaults the Texture name for content to _BaseMap instead of _BaseColorMap, so the books should work fine in URP and HDRP after the materials have been converted.

v1.12
Fixed an error if the inspector was made wider than the MegaBook logo image.
Added warnings for when you use a custom page mesh to inform if the mesh has enough materials and is readable.
Fixed an issue if you tried to add a mesh as content that had no uv mapping at all.

v1.11
Fixed an exception which would happen if you set the number of pages to more than 30 in the Book Create Window.

v1.10
Fixed a bug where if the book was rebuilt at runtime the pages would not be set to the correct position until the page value was changed.

v1.09
Added support for RenderLayerMask for the page meshes.

v1.08
Rewrote the system that finds exisiting book and cover styles in a project for the CreateBookWindow, in big projects the old way was too slow.
Updated the same system in the Boko Editor script as well for cover styles
You can now click the foldout bars to expand inspector sections rather than just the arrow

v1.07
Removed the GameObject/Create Other/MegaBook option left over from MB1, to create a book use the 'Window/Create Book/ window.
Cover finder now doesn't use a fixed folder for finding cover styles in the project will find them in whole project
The above issue meant the MegaBook folder could not be moved in a project without causing an error that is now fixed.
If you are not using the builtin pipeline the BookCamera.cs script may give errors about PostProcessing, if comment out the first line to fix that

v1.06
Made MegaBook 2 compatible with Unity 2023
Made MegaBook 2 compatible with Unity 2022

v1.05
Added an 'active' option to the Page Objects, if you need to control the object in somewhere set this to active to stop the book controlling it
Added a Layer system for the Page Objects, allowing you to have different layers of objects that can be turned on and off by using the page front and back layerid values
Noticed a jitter in the mesh when turning a page very slowly when it first starts caused by large bend angle values, now def system will auto switch to a double instead of float job if it needs extra precision

v1.04
PageParams follow copied when making a new page
Fixed hole mesh being shown if prev page was a reduced width custom page mesh
Further tooltips added to Foldouts, buttons and other missing params, each value, button etc now has its own tooltip to explain it.
Further improvements to workflow and inspector
Adjusted inspector sensitivity of some params for easier editing
Fixed when undoing headband and spine fabric changes the meshes would update to the new values
Fixed an issue when recording an animation of the book being read with the Page value not playing back correctly.

v1.03
Added a cancel progress bar to the 'Make Pages' button as it can be a little slow.
Each page can now have its own Mask texture if making textures for that page is on.
Improvements to the layout of the 'Page' section of the inspector to make it clearer
Each page can now override the Book collider option to can set per page if collider is generated and updated
Option to subdivide Text Mesh Pro meshes, useful if large text is clipping through a turning page.
Each page can override the book Text Mesh Pro setting
If using custom meshes for pages added a Pivot value in case your custom mesh has a pivot position in a differnt place
Mesh content can now be added to pages that use custom meshes
Fixed a rendering issue where wrong edge material could be shown of pages in middle of book.
Added Custom page meshes effected by Noise values.
Added tooltips the various buttons in the inspector and a few other params that were missing them.
Page textures now automatically update when you change their properties

v1.02
Fixed a bug when using the 'Set' buttons for Object visibility when editing the first page, the values would be set wrongly.
Added support for fading in and out UI objects if the object has a CanvasGroup component attached
Add options to the Page settings to control the Width and length segs used to make a page mesh. If values are > 0 then those values will used for that page, if 0 or less then the book values will be used
Fixed false message regarding memory leaks when entering and exiting play mode
Automated the detection of which Pipeline is being used and setting the texture name value accordingly, this can be overruled by the shader texture name still for custom shaders
Relocated the 'Use Alpha as Mask' check box to its correct position under the Back textures section
Changed and improved the Mesh content system, no longer have to add MCComplexPage to an object to get the content, now each page allows you to pick the objects you want to add and each one can have its own scaling, rotation and position

v1.01
Put limit of 1 on low values for the height,width and length seg counts
Fixed a small bug if spine radius set to 0 causing headband to go wrong
Added a wire gizmo to show the new size of the book when dragging the size handles
Fixed error if Use Hole Mesh option was turned off
New option to highlight the page being edited.
Added Scale Mesh Width option to the page param, this is used if you select a custom mesh for the page, useful for say a torn out page.
Added support for video content for pages to the system
You can now select a PhysicMaterial to apply to any page colliders

v1.0
Megabook 2 rewitten to use Burst and Jobs for big speed improvements.
Editor code rewritten for better performance.
Undo fully supported.
New demo scene.
All code for older versions of Unity removed streamlining the code.
Code all in MegaBook namespace
Includes example old books in different styles to get you started.
Books can now use the new cover system which uses skinned meshes for much nicer looking books.
Built in skinning of your own book covers utility.
Adjust the cover pivot locations, rotation axism scaling to suit your needs.
More control over the page spread animation for better looking books.
Added a Turn slider for finer control of page turn while editing the book.
Attached object system improved, can now add scaling, translation and rotation to have objects appear and vanish nicely.
Attached object offset can be controlled by a curve as the page turns to stop poke through off objects.
New 'BookAppeared' and 'BookVanished' messages for when attached objects are turned on and off by the book.
New 'BookAppear' and 'BookVanish' messages which pass the amount the object is appearing or vanishing so you can control effects like fade in/out something.
Can add Unity UI world space elements to pages using the attach system.
All key params have tooltips to explain what they do.
The spine radius can now be controlled via a curve as the book is read so the curve of the pages at the binding can change.
Added headbands option so the ends of the pages at the spine can have a decorative cover as found in old books.
The headband will deform to match the pages spine radius so will change along with that.
Added Spine fabric option so you can cover the ends of your pages nicely, deforms corectly when the book is read.
New animation option control the max and min angles as the book turns.
Example scripts showing use of book messages to control objects attached to a page.
Noise option for page mesh creation to add rough look to pages, full control over how much and where noise is added.
Create Book wizard, select a few options and click create. Select from book styles and cover styles.
Easily add your own book and cover styles to the system.
Improved page edge UV options.
Autofit covers from one size book to another book
Autodisabling of page turners so no CPU used it pages not turning, have dozens of books in the scene with no extra CPU usage.
Includes old antique style blank page textures
Several 3d assets included, such as book props, table, candlestick, inkwell etc.
Numerous other small improvements and optimizations.
Book Shelf Filler Helper Script

Book styles
Thick Small
Thick Wide
Thin Small
Thin Wide
Normal Small
Normal Wide
Big Tomb

Cover Styles
Illustrated
Skull Crossbones
Fabric
Leather Brown
Leather Green
Leather Purple
Leather Blue
Leather Grey

Pages
Pirate
Ship
Map
Text
Plain
Plain 1
Plain 2
Plain 3
Plain 4

Spine Fabric
Headband
Headband Red
Headband Green
Headband Blue
Headband Brown

Book Objects
Open Illustrated
Skull Crossbones
Fabric
Leather

Props
Table
Tankard
Inkwell
Candlestickand Candle
Quill
Run Bottle
Gold Doubloon






