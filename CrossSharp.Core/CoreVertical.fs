namespace CrossSharp.Core

module CoreVertical =

    open CoreHorizontal
    open System

    let hasrightchar (board:string[,]) (row:int) (col:int) = 
        match col with
        | col when col < 1 -> false
        |_ -> 
            let nextcol = col + 1
            board.[row, nextcol] <> emptyCell

                
    let hasleftchar (board:string[,]) (row:int) (col:int) = 
        match col with
        | col when col < 1 -> false
        |_ -> 
            let nextcol = col - 1
            board.[row, nextcol] <> emptyCell

    let verNeighboursAreNotEmpty (board:string[,]) rowpos col = 
        let topcell = rowpos-1
        let bottomcell = rowpos+1
        if((board.[topcell, col].ToString() = emptyCell)
                        && (board.[bottomcell, col].ToString() = emptyCell)) then 
            false
        else
            true     

    let rec findVerticalMatch ( wordchars:char[]) (board:string[,]) startrow startcol letterindex (result: (bool * matchingCell[])) =
        if (letterindex < wordchars.Length && startrow < 15 && startcol < 15) then
            if board.[startrow,startcol] = wordchars.[letterindex].ToString() then
                let noInvalidcharAbove = hastopchar board startrow startcol
                let noInvalidcharBelow = hasbottomchar board startrow startcol
                let noVerCharBeforeOrAfter = verNeighboursAreNotEmpty board startrow startcol
                let cells = snd result
                if(noInvalidcharAbove = false && noInvalidcharBelow = false && noVerCharBeforeOrAfter = false) then
                    cells.[letterindex] <- {matchingCell.row = startrow; matchingCell.col = startcol; matchingCell.letterindex = letterindex}
                    findVerticalMatch wordchars board (startrow + 1) startcol (letterindex + 1)  (true, cells)
                else
                    result
            elif board.[startrow,startcol] = emptyCell then
                findVerticalMatch wordchars board (startrow + 1) startcol (letterindex + 1) result
            else
                result
        else
            result
    let cellHasHorizontalNeighbours (board:string[,]) (acell:matchingCell) = 
            let hasAbove = board.[(acell.row), (acell.col - 1)] = emptyCell
            let hasBelow = board.[(acell.row), (acell.col + 1)] = emptyCell
            hasAbove && hasBelow


    let rec NoVerticaCellsHaveHorizonalNeighbours (board:string[,]) (cells:matchingCell[]) index =
        if(cells.Length = (index + 1)) then
            true
        elif(not(cellHasHorizontalNeighbours board cells.[index])) then
            false
        else
            NoVerticaCellsHaveHorizonalNeighbours  board cells (index + 1)

    //Validate that entire word does not trip over any other on the board.
    let validForVertical (result: (bool * matchingCell[])) (wordchars:char[]) (board:string[,]) = 
        if not (fst result) then 
            false
        else
            let cells = snd result

            let firstmatchedcell =  cells |> Array.find(fun x -> x.letterindex >= 0)
            let thefirstmatchingrow = firstmatchedcell.row - firstmatchedcell.letterindex
            //Ensure cell after last matching position for word is empty.
            if (board.[(thefirstmatchingrow + wordchars.Length), firstmatchedcell.col] = emptyCell) then
                //get position for unmatched chars on the board based on the match
                let placeholderarray = Array.zeroCreate(wordchars.Length)
     
                cells |> Array.iteri(fun i x -> 
                                            if(x.letterindex = -1 ) then
                                                let currentrow = thefirstmatchingrow  + i 
                                                let cell = {matchingCell.row = currentrow; matchingCell.col = firstmatchedcell.col; matchingCell.letterindex = i}
                                                placeholderarray.[i] <- cell
                                            else
                                               let blankcell = {matchingCell.row = -1; matchingCell.col = -1; matchingCell.letterindex = i}
                                               placeholderarray.[i] <- blankcell) 

                //At this point placeholderarray will contain position for chars that have not been matched against.
                let unmatchedCharsCells = placeholderarray |> Array.filter(fun x -> x.row >= 0 )
                       
                NoVerticaCellsHaveHorizonalNeighbours board unmatchedCharsCells 0
            else
                false

    let nextCell cell = 
        match cell with
        |(14, 14) -> (-1,-1)
        |(row, col) & (_,14) -> ((row + 1), 0)
        |(row, col) & (_,_) -> (row, (col + 1))

    let  rec boardloop (board:string[,]) row col (wordchars:char[]) = 
        let next = nextCell (row,col)
        let newrow = fst next
        let newcol = snd next
        if (newrow = -1) then
            false
        else
            let wordfound = resultTuple wordchars.Length |> findVerticalMatch wordchars board row col 0 
            if validForVertical wordfound wordchars board then 
                //Add the word to the board
                let cells = snd wordfound
                getStartPosFromRecordResult cells Orientation.vertical |> addword wordchars board Orientation.vertical
                true
            else
                boardloop board newrow newcol wordchars



    let AddWordVertically (word:string) (board:string[,]) = 
        let wordchars = word.ToCharArray()
        boardloop board 0 0 wordchars