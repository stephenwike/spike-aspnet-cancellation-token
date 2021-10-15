# Spike - CancellationToken

This spike is aimed at looking at how cancellation tokens work with ASP.NET Web APIs.

## Running the Example

Run the project from visual studio.  On the loaded website, there are two buttons.  Both buttons utilize a "workflow" that takes 10 seconds to complete.  A message will appear on the bottom of the page notifying the client is the workflow was completed or cancelled.

### Nonstop button

This button will run the workload through its entire duration unless cancelled by the client.

### Work2Secs button 

This button will cancel the workload after two seconds.