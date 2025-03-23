Considerations & change summary:

DELETION:

1. Added Delete User functionality
--> this is done using User Id however. To facilitate future usability I have changed the Id to string type
   to enable use of UUID as suggested in our last meeting.
--> all changes made were performed in any relevant layers of the API (Domain Model, Controller, UserRepo & Infra)

2. We should consider changing all controllers to find Users by Id to facilitate finding only unique users.
--> then we can also get rid of the GetByEmailAsync method.



UPDATE:

1. Added update functionality. Most notably also added DTO object and Interface for it to allow patching of user account data without having to update the entire user account for each request --> why?: HTTP Update requires all information to be 'changed', while HTTP Patch allows selective updates
--> This will require front-end adaptation to ensure that the Patch request sends the correct data (as with the other functions current user password is required to authenticate)

NOTE: all other data contained in the UserUpdateDto is nullable allowing selective updating --> password is still required and not nullable!
