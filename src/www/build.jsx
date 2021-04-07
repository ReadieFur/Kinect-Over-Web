//This file just has the .jsx extension to make the .gitignore easier.
require("log-timestamp");
const fs = require("fs");
const { resolve } = require("path");
const dirTree = require("directory-tree");

//I still need to get scss compiling in here, and I may as well do TSC while im at it.

//This class will be used to copy and move files to the output directory.
class CopyFiles
{
    static GetFiles(pathString)
    {
        const tree = dirTree(pathString);
        tree.children.forEach(child =>
        {
            if (child.type === "directory") { if (child.children !== undefined && child.children.length > 0) { CopyFiles.GetChildren(child.children); } }
            else { CopyFiles.WorkWithFile(child); }
        });
    }
    
    static GetChildren(children)
    {
        for (let i = 0; i < children.length; i++)
        {
            const child = children[i];
            if (child.type === "directory") { if (child.children !== undefined && child.children.length > 0) { CopyFiles.GetChildren(child.children); } }
            else { CopyFiles.WorkWithFile(child); }    
        }
    }
    
    static WorkWithFile(child)
    {
        if (mode === "Release" || mode === "dist" && (
            child.extension === ".ts" ||
            child.extension === ".scss" ||
            child.extension === ".map"
        ))
        { return; }
        
        var pathSplit = child.path.split("\\");
        var filePath = [];
        for (let i = 1; i < pathSplit.length - 1; i++)
        { filePath.push(pathSplit[i]); }
        filePath = filePath.join("/");
    
        CreateDirectoriesIfNotExists(`${outputPath}/${filePath}`);
    
        //Move the file.
        if (
            child.extension === ".js" ||
            child.extension === ".css" ||
            child.extension === ".map"
        )
        { fs.rename(`./src/${filePath}/${child.name}`, `${outputPath}/${filePath}/${child.name}`, LogError); }
        else //Copy the file.
        { fs.copyFile(child.path, `${outputPath}/${filePath}/${child.name}`, LogError); }
    }
}

//This class will be used to delte the compiled files from the src directory if there are any left over (usually on release, I could try to get TSC to compile with different configs instead in the future).
//I can impliment the above but I can't be arsed to change this file.
class DeleteFiles
{
    static GetFiles(pathString)
    {
        const tree = dirTree(pathString);
        tree.children.forEach(child =>
        {
            if (child.type === "directory") { if (child.children !== undefined && child.children.length > 0) { DeleteFiles.GetChildren(child.children); } }
            else { DeleteFiles.WorkWithFile(child); }
        });
    }
    
    static GetChildren(children)
    {
        for (let i = 0; i < children.length; i++)
        {
            const child = children[i];
            if (child.type === "directory") { if (child.children !== undefined && child.children.length > 0) { DeleteFiles.GetChildren(child.children); } }
            else { DeleteFiles.WorkWithFile(child); }    
        }
    }
    
    static WorkWithFile(child)
    {
        if (child.type === "file")
        {
            if (
                child.extension === ".js" ||
                child.extension === ".css" ||
                child.extension === ".map"
            )
            {
                var pathSplit = child.path.split("\\");
                var filePath = [];
                for (let i = 1; i < pathSplit.length - 1; i++)
                { filePath.push(pathSplit[i]); }
                filePath = filePath.join("/");

                if (fs.existsSync(`./src/${filePath}/${child.name}`))
                { fs.unlinkSync(`./src/${filePath}/${child.name}`); }
            }
        }
    }
}

function CreateDirectoriesIfNotExists(filePath)
{
    filePath = resolve(filePath);
    var pathSplit = resolve(filePath).split('\\');
    var pathString = pathSplit[0];
    for (let i = 1; i < pathSplit.length; i++)
    {
        pathString += `/${pathSplit[i]}`;
        if (!fs.existsSync(resolve(pathString))) { fs.mkdirSync(resolve(pathString)); }
    }
}

function DeleteDirectory(filePath)
{
    if (fs.existsSync(filePath))
    {
        const files = fs.readdirSync(filePath)
        if (files.length > 0)
        {
            files.forEach(function(filename)
            {
                if (fs.statSync(filePath + "/" + filename).isDirectory())
                { DeleteDirectory(filePath + "/" + filename) }
                else { fs.unlinkSync(filePath + "/" + filename) }
            })
            fs.rmdirSync(filePath)
        }
        else { fs.rmdirSync(filePath) }
    }
    //else directory not found.
}

function LogError(err) { if (err) { console.error(err); } }

//Run startup code below here.
var mode = "Release";
var outputPath = "./build";
//I am currently unsure if I want to distribute the web files with the app or keep them on my server.
//This script is not ideal for compiling files and is probably quite slow but it will do for now.
process.argv.forEach(argv =>
{
    //Debug copies all files to the /obj/debug/www folder. (Use with VS).
    if (argv == "--Debug") { mode = "Debug"; outputPath = `../obj/${mode}/www`; }
    //Release copies compiled files and resources to the /obj/release/www folder. (Use when compiling files to be distributed with the app). (Use with VS).
    if (argv == "--Release") { mode = "Release"; outputPath = `../obj/${mode}/www`; }
    //Publish copies compiled files and resources to the /www/dist folder. (Use when compiling for a public webserver). (Use with VSC).
    if (argv == "--Publish") { mode = "dist"; outputPath = `./${mode}`; }
    //Build copies all files to the /www/build folder. (Use with VSC).
    if (argv == "--Build") { mode = "build"; outputPath = `./${mode}`; }
});

if (fs.existsSync(outputPath)) { DeleteDirectory(outputPath); }
CreateDirectoriesIfNotExists(outputPath);
CopyFiles.GetFiles('./src/');
DeleteFiles.GetFiles('./src/');