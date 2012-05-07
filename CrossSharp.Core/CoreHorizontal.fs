// Learn more about F# at http://fsharp.net

namespace CrossSharp.Core

module CoreHorizontal = 

    open System

    let emptyCell = "_"

    type Orientation =
        | vertical = 0
        | horizontal = 1


    //Cell record type
    type matchingCell = {row:int; col:int; letterindex:int}
    type resultCell = {row:int; col:int; word:string; inserted:bool; orientation:Orientation}

    

    let  GetBoard  rows cols = Array2D.create rows cols emptyCell

    let sortFunction (first:string) (second:string) = 
        if(first.Length < second.Length) then
            1
        else if (first.Length > second.Length) then
            -1
        else
            String.Compare(first, second)


    let nextCell cell = 
        match cell with
        |(14, 14) -> (-1,-1)
        |(row, col) & (_,14) -> ((row + 1), 0)
        |(row, col) & (_,_) -> (row, (col + 1))

    let SortWords words = Array.sortWith sortFunction words


    //Add the first word is added at the top left side of the board
    let AddFirstWord (word:string) (board:string[,]) = 
        if(word.Length < Array2D.length2 board) then
            let wordChars = word.ToCharArray()
            for i in 0..word.Length-1 do
                board.[0,i] <- wordChars.[i].ToString() 
        board

    //Return position of matching vertical letter
    let MatchFirstLetterOfWord (wordchars:char[]) (board:string[,]) =
        let firstLetter = wordchars.[0].ToString()
        let hLength = Array2D.length1 board

        let rec GetLocationIfExists letter board len index =
            if(index = Array2D.length1 board) then
                -1
            elif (board.[0,index].ToString() = letter) then
                index
            else
                let newindex = index+1
                GetLocationIfExists letter board len newindex
        let hpos = GetLocationIfExists firstLetter board 15 0
        hpos


    //Second word section
    let horNeighboursAreNotEmpty (board:string[,]) row colpos = 
        let leftcell = colpos-1
        let rightcell = colpos+1
        if((board.[row, leftcell].ToString() = emptyCell)
                        && (board.[row, rightcell].ToString() = emptyCell)) then 
            false
        else
            true     

    let rec CanWordBeInsertedVertically(wordchars:char[]) (board:string[,]) (colpos:int) (pos:int) = 
        if(colpos < 0) then false
        elif(pos = wordchars.Length ) then 
            true
        else
            if(wordchars.[pos].ToString() = board.[pos, colpos] ||  board.[pos, colpos] = emptyCell) then
                let hasneighbours = horNeighboursAreNotEmpty board pos colpos 
                if(hasneighbours = false) then
                    let newPos = pos+1
                    CanWordBeInsertedVertically wordchars board colpos newPos     
                else
                    false
            else
                false

    let rec AddWordVertically (wordChars:char[]) (board:string[,]) col prevrow = 
        for i in 1 .. wordChars.Length-1 do
            let row = prevrow + i
            board.[row, col] <- wordChars.[i].ToString()

    let AddSecondWord  (word:string) (board:string[,]) =  
        let wordchars = word.ToCharArray()
        let positionOfFirst =  MatchFirstLetterOfWord wordchars board

        if(CanWordBeInsertedVertically wordchars board positionOfFirst 1) then
            AddWordVertically  wordchars board positionOfFirst 0          
            true
        else
            false


    //Vertical Words


    //Tuple of bool and cell Array passed into functions to record matching positions for new words. 
    let resultTuple length : (bool * matchingCell[]) = 
        let emptycell = {row = -1; col = -1; letterindex = -1}
        let cells = Array.create(length) emptycell
        (false, cells)

    //Given a board cell, check if cell vertically down is empty
    let hasbottomchar (board:string[,]) (row:int) (col:int) = 
        let newrow = row + 1
        let rowcount = Array2D.length1 board
        match row with
        | row when row = newrow || row = 0 -> false
        | _ ->  board.[newrow, (col- 1)] <> emptyCell

    //Given a board cell, check if cell vertically up is empty
    let hastopchar (board:string[,]) (row:int) (col:int) = 
            match row with
            | row when row < 1 -> false
            | _ -> 
                    let rowabove = row - 1
                    board.[rowabove, (col - 1)] <> emptyCell

    //Walk the board horizontally and return result in last param (matchingCell tuple). 
    //Validation is done only on matching chars, so a positive result may yet not be valid. 
    let rec findHorizontalMatch (wordchars:char[]) (board:string[,]) startrow startcol letterindex  (result: (bool * matchingCell[])) =
        if (letterindex < wordchars.Length && (startcol + wordchars.Length) < board.GetLength(1)) then
           if board.[startrow,startcol] = wordchars.[letterindex].ToString() then
                //Match or emptry cell
                //Does letter have any vertical neighbours ?
                let noInvalidcharAbove = hastopchar board startrow startcol
                let noInvalidcharBelow = hasbottomchar board startrow startcol
                let noHorCharBeforeOrAfter = horNeighboursAreNotEmpty board startrow startcol
                if(noInvalidcharAbove = false && noInvalidcharBelow = false && noHorCharBeforeOrAfter = false) then

                    let isfound = fst result 
                    let cells = snd result
                    let currentcell = {matchingCell.row = startrow; matchingCell.col = startcol; matchingCell.letterindex = letterindex}
                    cells.[letterindex] <- currentcell

                    let nextletter = letterindex + 1
                    let nextcol = startcol + 1
                    findHorizontalMatch wordchars board startrow (startcol + 1) (letterindex + 1) (true, cells)
                else
                    result
            elif board.[startrow,startcol] = "_" then
               findHorizontalMatch wordchars board startrow (startcol + 1) (letterindex + 1) result
            else
                result
        else
            result


    let cellHasVertNeighbours (board:string[,]) (acell:matchingCell) = 
            let hasAbove = board.[(acell.row - 1), (acell.col)] = emptyCell
            let hasBelow = board.[(acell.row + 1), (acell.col)] = emptyCell
            hasAbove && hasBelow


    let rec HasNoHorizontalCellsHaveVertNeighbours (board:string[,]) (cells:matchingCell[]) index =
        if(cells.Length = (index + 1)) then
            true
        elif(not(cellHasVertNeighbours board cells.[index])) then
            false
        else
            HasNoHorizontalCellsHaveVertNeighbours board cells (index + 1)

    //Validate that entire word does not trip over any other on the board.
    let validForHorizontal (result: (bool * matchingCell[])) (wordchars:char[]) (board:string[,])= 
        if not (fst result) then 
            false
        else
            let cells = snd result
            //get position for unmatched chars on the board based on the match
            let placeholderarray = Array.zeroCreate(wordchars.Length)

            let firstmatchedcell =  cells |> Array.find(fun x -> x.letterindex >= 0)
            let thefirstmatchingcoll = firstmatchedcell.col - firstmatchedcell.letterindex
            cells |> Array.iteri(fun i x -> 
                                        if(x.letterindex = -1 ) then
                                            let currentcol = thefirstmatchingcoll  + i 
                                            let cell = {matchingCell.row = firstmatchedcell.row; matchingCell.col = currentcol; matchingCell.letterindex = i}
                                            placeholderarray.[i] <- cell
                                        else
                                           let blankcell = {matchingCell.row = -1; matchingCell.col = -1; matchingCell.letterindex = i}
                                           placeholderarray.[i] <- blankcell) 

            //At this point placeholderarray will contain position for chars that have not been matched against.
            let unmatchedCharsCells = placeholderarray |> Array.filter(fun x -> x.row >= 0 )
                   
            HasNoHorizontalCellsHaveVertNeighbours board unmatchedCharsCells 0


    let getStartPosFromRecordResult (cells:matchingCell[]) direction =
        let firstcell = cells |> Array.find(fun x -> x.letterindex >= 0 )              
        if(direction = Orientation.horizontal) then
            (firstcell.row, (firstcell.col - firstcell.letterindex))
        else
            ((firstcell.row - firstcell.letterindex), firstcell.col)

    let addword (wordchars:char[]) (board:string[,])  (direction:Orientation) (position:(int * int)) = 
        let col = snd position
        let row = fst position
        let cnt = wordchars.Length
        for index in 0..wordchars.Length - 1 do
            match direction with
            | Orientation.horizontal ->  board.[row,col + index] <- wordchars.[index].ToString()
            | _ -> board.[row + index, col ] <- wordchars.[index].ToString()

//    //Given a board with the first two base entries (Horizontal and vertical), loop to find next horizontal match for given word.
//    //Add for valid position on board is found.
//    let rec loopboardrows (board:string[,]) (wordchars:char[]) row col  (result: (bool * matchingCell[])) = 
//        let wordlen = wordchars.Length
//        if((col + wordlen) < 15) then
//            let wordfound = findHorizontalMatch wordchars board row col 0  result
//            if validForHorizontal wordfound wordchars board then
//                //Add the word to the board
//                let cells = snd result
//                let firstcellformatchingword = getStartPosFromRecordResult cells Orientation.horizontal 
//                firstcellformatchingword |> addword wordchars board Orientation.horizontal
//                (true,firstcellformatchingword)
//            else
//                let newcol = col + 1
//                loopboardrows board wordchars row newcol result
//        else
//            //next row
//            let newrow = row + 1
//            if newrow < 15 then
//                loopboardrows board wordchars newrow 0 result
//            else
//                (false,firstcellformatchingword)

    let  rec boardloophoriz (board:string[,]) row col (wordchars:char[]) = 
        let next = nextCell (row,col)
        let newrow = fst next
        let newcol = snd next
        if (newrow = -1) then
            (false, (-1,-1))
        else
            let wordfound = resultTuple wordchars.Length |> findHorizontalMatch wordchars board row col 0 
            if validForHorizontal wordfound wordchars board then 
                //Add the word to the board
                let cells = snd wordfound
                let startPos = getStartPosFromRecordResult cells Orientation.horizontal 
                startPos|> addword wordchars board Orientation.horizontal
                (true, startPos)
            else
                boardloophoriz board newrow newcol wordchars


    let AddWordHorizontally (word:string) (board:string[,]) = 
        let wordchars = word.ToCharArray()
        boardloophoriz board 0 0 wordchars

    let rec buildrowstring rowindex colindex (board:string[,]) str =
        let rlen = Array2D.length1 board
        let clen = Array2D.length2 board
        if(clen = colindex) then
            str
        else
            let res = str + board.[rowindex, colindex] + " "
            let newcol = colindex + 1 
            buildrowstring rowindex newcol board res 

    let printboard (board:string[,])= 
        let rlen = Array2D.length1 board
        let clen = Array2D.length2 board
        let stringarray = seq { for row in 0..rlen-1 -> buildrowstring row 0 board "" }
        stringarray