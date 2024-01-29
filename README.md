# Rusty JS 🛠️⭐

Hi there! This is my compiler for Rusty JS! 
## What's Rusty JS?
> Rusty JS is a compiler wich allows you to write clean Rust look alike code.


### Examples:
```rust
    class Person
        pub mut name:str;
        priv mut age:i32;
        
        fn _init_(name:str, age:i32)
            $.name = name;
            $.age = age;
        end
    end
```
```rust
    log!("Hello world!");
    _doc.query_all('p');
    _doc.query('p');
```
```
    fn add(x:f32,y:f32)
        ret x + y;
    end
```
> JS
```js
    class Person {
        constructor(name,age) {
            this.name = name;
            this.age = age;
        }
    }
```
```js
    console.log("Hello world!");
    document.querySelectorAll('p');
    document.querySelector('p');
```
```js
    function add(x,y) {
        return x + y;
    }
```

#### Commands
`RSC -v`
`RSC -f file.rsjs`
`RSC -f file.rsjs -o build.js`

![logo-rsjs](https://github.com/Just-a-Jason/Rusty-JS-Compiler/assets/88512392/17cb5674-1559-4dcc-9ecf-8b9298fa7d7f)

> It's still in progress!﻿
