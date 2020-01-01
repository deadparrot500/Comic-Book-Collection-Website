### Thanks
Thank you for taking the time to review my code.  I would appreciate any feedback you have.  Root is an amazing organization that I would love to join!

### Language
I had intended to write this program using Javascript and Vue.js, mainly because it was I wanted to gain a deeper understanding of that language and structure.
However, after digging into the problem statement and carefully considering my approach I choose C# MVC for two main reasons. 
One, it is the language and structure I know best. 
And two, it provides an ease of writing and running tests that Vue.js does not.

### Basic Structure 
This program was written on Visual Studio Code and I ran it using the built-in IIS Express functionality.  
I used much of the framework that Microsoft Provides when generating a new MVC program.  
I also left the Carousel banner on the first page for some visual interest.

The Home View generates a basic form that the user uses to input a file path.
That file path is passed to the Results Controller which in turn passes it to the FileReader in the Data Access Layer.
The FileReader reader class takes in a file path from the Results Controller, reads the file, and returns a sorted list of Drivers back to the Results Controller.
The Results Controller passes that back to the Results View for display.

Because the problem statement specified that no data needed to be saved in a persistent state, I felt that it would be best to avoid using SQL or any database applications.
The only methods in the Data Access Layer class are related to the FileReader, where otherwise you would find all the code related to accessing a database.
I included all the logic in that file as well so it would be easy to test. 
I also included an interface for the FileReader, following good practice.

I made the decision when writing this program, to allow a Driver to be added through a Trip line, even if there was not a Driver line with that name.
I felt that this would make the program less likely to break and fail in the event that someone mistakenly included a Trip without first registering a Driver.
This required a conditional statement added to the AddATrip method to check for an existing Driver.  
The DriverNameList property that was added to the FileReader is used to check for existing Drivers when adding either a Driver or a Trip.  
The idea is that the DriverNameList and the DriverList will always be synchronized. 

I built a Model called Driver to hold the Data related to the drivers.  
This includes a couple properties that are derived using inputs from the file.  
I wrote tests to ensure that the derived properties were working correctly.


### Testing
I wrote a test for each method in the FileReader. 
My practice was to right the method and then to immediately write the test for it.  
I wrote the method first as often I was not always sure what arguments the method would need until it was written.

I used the tests in two ways.
First I used the tests as I was building the program to ensure that each piece was working before I moved on to the next piece.
Second I used them all as a whole at the end to ensure the program still worked as intended with each little refactor.





-SQL Avoidance
-Why much of the MS framework remains
-Why a list of models
-Why 3 lists (name of drivers, driver list, sorted driver list)

Test Strategy
-Both to test completed code and to test components as built
-Test every method
-Test every derived property in model

