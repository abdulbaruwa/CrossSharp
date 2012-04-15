// Learn more about F# at http://fsharp.net

open System



let board  = Array2D.create 15 15 "_"

let words = [|"Adedayo" ;"Ademola"; "Aluko"; "Bamidele"; "Yetunde"|]

let sortFunction (first:string) (second:string) = 
    if(first.Length < second.Length) then
        1
    else if (first.Length > second.Length) then
        -1
    else
        String.Compare(first, second)


let sortWords words = Array.sortWith sortFunction words
 
let sortedWords = sortWords words


let AddFirstWord (word:string) (board:string[,]) = 
    if(word.Length < Array2D.length2 board) then
        let wordChars = word.ToCharArray()
        for i in 0..word.Length-1 do
            board.[0,i] <- wordChars.[i].ToString() 

//Go through first row looking for first letter of next word.
let MatchFirstLetterOfWord (word:string) (board:string[,]) =
    let wordChars = word.ToCharArray()
    let firstLetter = wordChars.[0].ToString()
    let hLength = Array2D.length1 board

    let rec GetLocationIfExists letter board len index =
        if(index = Array2D.length1 board) then
            printfn "-1"
            -1
        elif (board.[0,index].ToString() = letter) then
            printfn "First Letter found and horizontal index position %d" index
            index
        else
            let newindex = index+1
            GetLocationIfExists letter board len newindex
    let hpos = GetLocationIfExists firstLetter board 15 0
    hpos

let horNeighboursAreEmpty (board:string[,]) pos colpos = 
    let leftcell = colpos-1
    let rightcell = colpos+1
    if((board.[pos, leftcell].ToString() = String.Empty)
                    && (board.[pos, rightcell].ToString() = String.Empty)) then 
        false
    else
        true     

let rec MatchSecondToLastLettersVertically (wordchars:char[]) (board:string[,]) (colpos:int) (pos:int) = 
    if(pos = wordchars.Length ) then 
        true
    else
        if(wordchars.[pos].ToString() = board.[pos, colpos] ||  board.[pos, colpos] = String.Empty) then
            let hasneighbours = horNeighboursAreEmpty board pos colpos 
            if(hasneighbours = false) then
                let newPos = pos+1
                MatchSecondToLastLettersVertically wordchars board colpos newPos     
            else
                false
        else
            false

let rec AddWordVertically (wordChars:char[]) (board:string[,]) col prevrow = 
    for i in 1 .. wordChars.Length-1 do
        let row = prevrow + i
        board.[row, col] <- wordChars.[i].ToString()

let rec buildrowstring rowindex colindex str =
    let rlen = Array2D.length1 board
    let clen = Array2D.length2 board
    if(clen = colindex) then
        str
    else
        let res = str + board.[rowindex, colindex] + " "
        let newcol = colindex + 1 
        buildrowstring rowindex newcol res

let printboard (board:string[,])= 
    let rlen = Array2D.length1 board
    let clen = Array2D.length2 board
    for row in 0..rlen-1 do 
        let str = buildrowstring row 0 ""
        Console.WriteLine(str)


 //Horizontal add
 //for each letter of word 
 // do i have a match on the board 
 // Yes (if No exit)
 // what is the index of the matching letter
 // Any neighbours at start and end of whole word? 
 // No (if yes exit)
 // Any clashes with existing words right and let of matching letter.
 //No (if yes exit)
 // Any neighbours vert (up and below) of remaining letters i.e letters right and center of matching letter. 
 //No (if yes exit)

let rec findHorizontalMatch (wordchars:char[]) (board:string[,]) startchar = 
        
 
AddFirstWord "Bamidele" board        

let posOfFirst = MatchFirstLetterOfWord "india" board
let wordchars = "india".ToCharArray()
let wordInsertedVertically = MatchSecondToLastLettersVertically wordchars board posOfFirst 1
AddWordVertically wordchars board posOfFirst 0          

printboard board

Console.ReadKey()



 