# FamilySearch Model Extensions

This is where the Java classes that correspond to the FamilySearch 

extensions to GEDCOM X live.

# NuGet Package Information


| Package ID          | Link                |
|---------------|---------------------------|
| Insert package name | insert package link |

See [the section on using these libraries](../README.md#Use).
Insert content when ready.

To determine the latest version, ~ visit the NuGet link listed above ~.

# Use

## Building the Model

Here's a sample of FamilySearch model extensions you might use: 

```csharp
//create a person.
Person person = new Person() //create a person
  .SetName("...something...") //named something
  .SetGender(GenderType.Male) //male
  .SetFact(new Fact(FactType.Birth, "...sometime...", "...someplace")); 

//born sometime, someplace

Discussion discussion = new Discussion()//create a discussion
  .SetTtle("...some title..."); //with some title.

DiscussionReference discussionReference = new DiscussionReference() 

//create a reference
  .SetResource(...); //to a discussion.
person.AddExtensionElement(discussionReference); //add a discussion reference to the person.

User user = new User(); //create a FamilySearch user.

ChildAndParentsRelationship childAndParentsRelationship = new 

ChildAndParentsRelationship() //create a child-and-parents 

relationship
  .SetChild(child) //between a child
  .SetFather(father) //a father
  .SetMother(mother); //and a mother

FamilySearchPlatform doc = new FamilySearchPlatform()
  .SetDiscussion(discussion)
  .SetUser(user)
  .SetChildAndParentsRelationship(childAndParentsRelationship)
  .SetPerson(person);
```

## XML 

Here's how you might write out some FamilySearch XML:

```csharp

FamilySearchPlatform fsp = ...; //construct the document
Stream stream = ...; //figure out where you want to write the XML
 
DefaultXmlSerialization serializer = ...;
serializer.Serialize(fps, stream);

```
## XML
Here's how you might read some FamilySearch XML:

```csharp
Stream stream = ...; //find the XML
 
DefaultXmlSerialization serializer = ...;
 
//read the document
FamilySearchPlatform fsp = serializer.Deserialize<FamilySearchPlatform>(stream)

```

## JSON  

Here's how you might write out some FamilySearch JSON:

```csharp
FamilySearchPlatform fsp = ...; //construct the document
Stream stream = ...; //figure out where you want to write the JSON
 
DefaultJsonSerialization serializer = ...;
 
//write the document to the stream:
Serializer.Serialize(fps, stream);

```

## JSON  

Here's how you might read some FamilySearch JSON:

```csharp
Stream stream = ...; //find the JSON
 
DefaultJsonSerialization serializer = ...;
 
FamilySearchPlatform fsp = serializer.Deserialize<FamilySearchPlatform>(stream);
```