# System.IO.Pipelines

Pipelines are supposed to help with network I/O

Let's make a super simple webserver to see this in action.

It will respond to GET, dumping the whole message to the console
and then respond with `{ "ok": true }`
