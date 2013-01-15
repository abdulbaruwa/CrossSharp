namespace CrossSharp.Core

module CoreVertical =

    open CoreHorizontal
    open System
    open System.Diagnostics
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
        if (topcell < 0 || (bottomcell >= Array2D.length2 board)) then
            false
        elif((board.[topcell, col].ToString() = emptyCell)
                        && (board.[bottomcell, col].ToString() = emptyCell)) then 
            false
        else
            true     


    //let rec findVerticalMatch ( wordchars:char[]) (board:string[,]) startrow startcol letterindex (result: (bool * matchingCell[])) =
    let rec findVerticalMatch (wordchars:char[]) (board:string[,]) rowIterator (startcell:(int*int)) letterindex  (result: (bool * matchingCell[])) =
        let startcol = snd startcell
        if (letterindex < wordchars.Length && (fst startcell + wordchars.Length) <= board.GetLength(1) && rowIterator < board.GetLength(1)) then
            match board.[rowIterator,startcol] with
            | x when x.Equals (wordchars.[letterindex].ToString(), StringComparison.OrdinalIgnoreCase) -> 
                        let noInvalidcharAbove = hastopchar board rowIterator startcol
                        let noInvalidcharBelow = hasbottomchar board rowIterator startcol
                        let noVerCharBeforeOrAfter = verNeighboursAreNotEmpty board rowIterator startcol
                        let cells = snd result
                        if(noInvalidcharAbove = false && noInvalidcharBelow = false && noVerCharBeforeOrAfter = false) then
                            cells.[letterindex] <- {matchingCell.row = rowIterator; matchingCell.col = startcol; matchingCell.letterindex = letterindex}
                            findVerticalMatch wordchars board (rowIterator + 1) startcell (letterindex + 1)  (true, cells)
                        else
                            result
            | x when x = emptyCell -> findVerticalMatch wordchars board (rowIterator + 1) startcell (letterindex + 1) result
            |_ -> (false, snd result)
        else
            result

                
    let cellHasHorizontalNeighbours (board:string[,]) (acell:matchingCell) = 
        
        let ccol = acell.col
        let rrow = acell.row
        let boardLength = (Array2D.length2 board)
        //System.Diagnostics.Debug.WriteLine(ccol.ToString())
        //System.Diagnostics.Debug.WriteLine(rrow)
        let celll = board.[rrow,ccol]

        match acell with
        | x when x.col = 0 -> false
        | x when x.col >= boardLength -> false
        | x -> (board.[(x.row), (x.col - 1)] = emptyCell) &&  (if (ccol < boardLength-1) then (board.[(x.row), (x.col + 1)] = emptyCell) else true) //ignore second part if at last col
        

        
    let cellsBeforeAndAfterWordAreEmpty  (board:string[,]) row col = 
        match row with
        | 0 -> true 
        | x when board.[(x - 1), col] = emptyCell -> true
        |_ -> false

    let rec NoVerticaCellsHaveHorizonalNeighbours (board:string[,]) (cells:matchingCell[]) index =
        if((index + 1) > cells.Length) then
            true
        elif(not(cellHasHorizontalNeighbours board cells.[index])) then
            false
        else
            NoVerticaCellsHaveHorizonalNeighbours  board cells (index + 1)

    let rec checkIfLettersOverlapAnyExistingLettersOnBoard firstmatchingrow (firstmatchedcell:matchingCell) (board:string[,]) (wordchars:char[]) counter =
        if (counter = wordchars.Length) then
            true
        elif ( (board.[(firstmatchingrow + counter), firstmatchedcell.col] <> wordchars.[counter].ToString() &&  board.[(firstmatchingrow + counter), firstmatchedcell.col]  <> "_")) then
            false
        else
            checkIfLettersOverlapAnyExistingLettersOnBoard firstmatchingrow firstmatchedcell board wordchars (counter + 1)

    //Validate that entire word does not trip over any other on the board.
    let validForVertical (result: (bool * matchingCell[])) (wordchars:char[]) (board:string[,]) = 
        if not (fst result) then 
            false
        else
            let cells = snd result

            let firstmatchedcell =  cells |> Array.find(fun x -> x.letterindex >= 0)
            let thefirstmatchingrow = firstmatchedcell.row - firstmatchedcell.letterindex
            let thelastmatchingrow = thefirstmatchingrow + wordchars.Length - 1

            if(thefirstmatchingrow > 0 && board.[(thefirstmatchingrow - 1), firstmatchedcell.col] <> emptyCell) then
                false
                //Ensure cell after last matching position for word is empty.
            elif ((thelastmatchingrow + 1 < board.GetLength(1)) && board.[(thelastmatchingrow + 1), firstmatchedcell.col] = emptyCell) then
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
                
                let hasNoOverlappingValuesInCells = checkIfLettersOverlapAnyExistingLettersOnBoard thefirstmatchingrow firstmatchedcell board wordchars 0 
                //(cellsBeforeAndAfterWordAreEmpty board thefirstmatchingrow firstmatchedcell.col) &&
               
                hasNoOverlappingValuesInCells && (NoVerticaCellsHaveHorizonalNeighbours board unmatchedCharsCells 0)
            else
                false
                
    let rec boardloop (board:string[,]) row col (wordchars:char[]) = 
        let next = nextCell (row,col)
        let newrow = fst next
        let newcol = snd next
        if (newrow = -1) then
            (false, (-1,-1))
        else
            let wordfound = resultTuple wordchars.Length |> findVerticalMatch wordchars board row (row, col) 0 
            if validForVertical wordfound wordchars board then 
                //Add the word to the board
                let cells = snd wordfound
                let startPos = getStartPosFromRecordResult cells Orientation.vertical 
                startPos|> addword wordchars board Orientation.vertical
                (true, startPos)
            else
                boardloop board newrow newcol wordchars


    let rec AttempToAddSecondWord board (words:string[]) index = 
        if index >= words.Length then 
            (-1,-1)
        else
            let resultofsecond = AddSecondWord words.[index] board
            if fst resultofsecond then
                let colforsecondword = snd resultofsecond
                //result tuple = (col on board where word will be inserted vertically * index for word in word list)
                (colforsecondword, index)         
            else
                AttempToAddSecondWord board words (index + 1)
    
    let AddWordVertically (word:string) (board:string[,]) = 
        let wordchars = word.ToCharArray()
        let vertresult = boardloop board 0 0 wordchars
        let cell = snd vertresult
        {resultCell.row = (fst cell); resultCell.col = snd cell; word = word; inserted = fst vertresult; orientation = Orientation.vertical}



    //Given remaining words (3rd onwards) loop through mapping a function to add to board and
    //return an array of (word, cell, orientation). If word is not added to board the start cell value should be set to (-1,-1)
    let addWordVerticallyOrHorizontally (word:string) (board:string[,]) = 
       let vertres = AddWordVertically word board
       if vertres.inserted then
            vertres
       else
            AddWordHorizontally word board     

    let AddRemainingWords (words:string[]) (board:string[,]) = 
            let thirdwordonwardsreslt = words |> Array.map (fun seqword -> addWordVerticallyOrHorizontally seqword board)
            (board, thirdwordonwardsreslt)

    let AddWords (words:string[]) (board:string[,]) =
        let sortedWords = SortWords words
        let board2 = AddFirstWord words.[0]  board
        let firstwordresult = {resultCell.row = 0; resultCell.col = 0; word = words.[0]; inserted = true; orientation = Orientation.horizontal}

        let wordsafterfirst = words.[1..]
        //Add second word, loop from second in list of words
        let secondwordindex = AttempToAddSecondWord board wordsafterfirst 0 

        let secondwordresult = if fst secondwordindex > -1 then
                                    {resultCell.row = 0; resultCell.col = fst secondwordindex; word = wordsafterfirst.[snd secondwordindex] ; inserted = true; orientation = Orientation.vertical}
                               else
                                    {resultCell.row = 0; resultCell.col = 0; word = String.Empty; inserted = false; orientation = Orientation.vertical}

        let resultsforfirst2items = [|firstwordresult ; secondwordresult |]


        if fst secondwordindex > -1 then
            let thirdwordonwards = [| for index in 0..(wordsafterfirst.Length - 1) do //builds sequence, filtering out the second word added on to the board.
                                                if(not (wordsafterfirst.[index] = wordsafterfirst.[snd secondwordindex])) then
                                                    yield wordsafterfirst.[index]|]
            //for the remaining words, attempt to add them to board vertically or horizontally.
            //Map a function to the sequence creating a new sequence of results (result is a record resultCell type). 
            let thirdwordonwardsreslt = AddRemainingWords thirdwordonwards board2
                                                                                                    
            (board2, (Array.append resultsforfirst2items (snd thirdwordonwardsreslt)))
        else
            (board2, resultsforfirst2items)



//
//    let AddMinimumTenWords (words:string[]) (board:string[,]) = 
//        let firstAttempt = AddWords words board


    let AddWordsInThreeIterations (words:string[]) (board:string[,]) = 
        let firstAttempt = AddWords words board
        let getUnenteredWords (resultsArray:resultCell[]) = [|for i in 0..(resultsArray.Length-1) do 
                                                                    if resultsArray.[i].inserted = false then yield resultsArray.[i].word |]
        
        let secondAttempt = AddRemainingWords (getUnenteredWords (snd firstAttempt)) (fst firstAttempt)
        let thirdAttempt = AddRemainingWords (getUnenteredWords (snd secondAttempt)) (fst secondAttempt)
        let filterInserted input =  
            input |> Array.filter(fun x ->  x.inserted = true)

        let result1 = filterInserted (snd firstAttempt)
        let result2 = filterInserted (snd secondAttempt)  
        let result3 = filterInserted (snd thirdAttempt)
        let finalResult =  Array.append result1 result2 |> Array.append result3
        (finalResult, snd thirdAttempt)


    let AddWordsAttempts (words:string[]) (board:string[,]) = 
        let firstAttempt = AddWords words board
        let getUnenteredWords (resultsArray:resultCell[]) = [|for i in 0..(resultsArray.Length-1) do 
                                                                    if resultsArray.[i].inserted = false then yield resultsArray.[i].word |]
        
        let secondAttempt = AddRemainingWords (getUnenteredWords (snd firstAttempt)) (fst firstAttempt)
        let thirdAttempt = AddRemainingWords (getUnenteredWords (snd secondAttempt)) (fst secondAttempt)
        let filterInserted input =  
            input |> Array.filter(fun x ->  x.inserted = true)

        let result1 = filterInserted (snd firstAttempt)
        let result2 = filterInserted (snd secondAttempt)  
        let result3 = filterInserted (snd thirdAttempt)
        let finalResult =  Array.append result1 result2
        (finalResult, fst thirdAttempt )

