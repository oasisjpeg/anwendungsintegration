Main considerations & change summary:

1. Added Delete User functionality
--> this is done using User Id however. To facilitate future usability I have changed the Id to string type
   to enable use of UUID as suggested in our last meeting.
--> all changes made were performed in any relevant layers of the API (Domain Model, Controller, UserRepo & Infra)

2. We should consider changing all controllers to find Users by Id to facilitate finding only unique users.
--> then we can also get rid of the GetByEmailAsync method.

   

to be continued...
