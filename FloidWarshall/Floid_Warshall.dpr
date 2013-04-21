{$APPTYPE CONSOLE}
Program Floid_Warshall;

uses
  SysUtils;

Const
  Infinity = 1000;

type
  TIntArray = Array of Integer;
  TTIntArray = array of array of Integer;
  //THistory = array of TTIntArray;

//Simply writes zeros in matrix
Procedure Zeros(var matrix : TTIntArray);
Var
  i, j, amount : Integer;
Begin
  amount := Length(matrix);
  For i := 0 to amount  - 1 do
    For j := 0 to amount - 1 do
      matrix[i, j] := 0;
End;

//Initialization
//We create initial matrix, read graph
Procedure Init(var matrix: TTIntArray);
Var
  f         : text;
  numOfNodes: Integer;
  i, j, k   : Integer;
Begin
  Assign(f, '3.txt');
  Reset(f);
  ReadLn(f, numOfNodes);

  SetLength(matrix, numOfNodes, numOfNodes);

  //Read elements into matrix
  For i := 0 to numOfNodes - 1 do
    For j := 0 to numOfNodes - 1 do
      Read(f, matrix[i,j]);

  CloseFile(f);
End;

//Init precendece matrix
Procedure InitPrecendenceMat(var prec: TTIntArray;
                             matrix: TTIntArray);
Var
  i, j, num: Integer;
Begin
  num := Length(matrix);
  SetLength(prec, num, num);

  for i := 0 to num - 1 do
    for j := 0 to num - 1 do
      //if ((i = j) or (matrix[i, j] < Infinity) then
      prec[i, j] := -1;
End;

//Finds minimum of two integers
Function Min(a : Integer; b : Integer): Integer;
Begin
  If (a <= b) then
    Result := a
  else
    Result := b;
End;

Procedure Floyd_Warshall(var matrix : TTIntArray;
                          var precedenceMat: TTIntArray);
Var
  i, j, k : Integer;
  n : Integer;
Begin
  //Num of steps and also num of nodes
  n := Length(matrix);

  For k := 0 to n - 1 do
    For i := 0 to n - 1 do
      For j := 0 to n - 1 do
      Begin
        if (matrix[i, k] + matrix[k, j] < matrix[i, j]) then
            precedenceMat[i, j] := k;

        matrix[i, j] :=
              Min(matrix[i, j],matrix[i, k] + matrix[k, j]);
       End;
End;

//Just print matrix
Procedure PrintMatrix(m: TTIntArray);
Var
  i, j, k : Integer;
Begin
  k := Length(m);

  WriteLn('***********');

  for i := 0 to k - 1 do
  Begin
    for j := 0 to k - 1 do
    Begin
      Write(m[i, j]);
      Write(' ');
    End;

    Writeln('');
  End;

  WriteLn('***********');

End;

Procedure PrintPath(p: TIntArray);
Var
  i : Integer;
Begin
  Write(' -> ');

  for i := 0 to Length(p) - 1 do
  Begin
    //In path all nodes starts with zero,
    //so we need to add 1 to restore ordinary count
    Write(p[i] + 1);
    Write(' -> ');
  End;
End;

//Reconstruct route from precedence matrix
//precedenceMat - матрица предшествования
//Recursively goes from i to j through
//intermediate nodes
Procedure ReconstructPath(i : Integer;
                          j : Integer;
                          matrix : TTIntArray;
                          precedenceMat : TTIntArray;
                          var path : TIntArray);
Var
  intermediate : Integer;
Begin
  //Path doesn't exist
  if (matrix[i, j] = Infinity) then
    Exit;

  intermediate := precedenceMat[i, j];

  //There exists path from i to j
  //through node "intermediate"
  //Else - we quit procedure
  if (intermediate <> -1) then
  Begin
    ReconstructPath(i, intermediate, matrix, precedenceMat, path);

    SetLength(path, Length(path) + 1);
    path[Length(path) - 1] := intermediate;

    ReconstructPath(intermediate, j, matrix, precedenceMat, path);
  End;

End;

Function PathContainsNode(node: Integer; path: TIntArray): Boolean;
Var
  i: Integer;
Begin
  Result := False;

  for i := 0 to Length(path) - 1 do
    if path[i] = node then
    Begin
      Result := True;
      Exit;
    End;
End;

procedure WriteGraph(FName: String; start: Integer; dest: Integer;
                      matrix: TTIntArray; path: TIntArray);
var
  F: TextFile;
  i, j: Integer;
  n: Integer;
begin
  AssignFile(F, FName);
  ReWrite(F);
  WriteLn(F, 'digraph G {');

  //Construct full route. Add start and destination
  SetLength(path, Length(path) + 2);
  for i := Length(path) - 2 downto 1 do
    path[i] := path[i - 1];

  path[0] := start;
  path[Length(path) - 1] := dest;
  //-------------------------

  n := Length(matrix);

  WriteLn(F, IntToStr(start + 1), ' [', 'shape = "diamond"', ', ',
          'color = "red"', ']');
  WriteLn(F, IntToStr(dest + 1), ' [', 'shape = "diamond"', ', ',
           'color = "red"', ']');

  //Write all nodes which are not in the route
  for i := 0 to n - 1 do
    for j := i to n - 1 do
      if (matrix[i, j] <> Infinity) and (i <> j) then
        if not PathContainsNode(i, path) or not PathContainsNode(j, path) then
          WriteLn(F, IntToStr(i + 1), ' -> ', IntToStr(j + 1),
             ' [label = "', IntToStr(matrix[i, j]), '"',
             ' dir = "none"', ']');

  //Write remaining nodes (route)
  for i := 0 to Length(path) - 2 do
    WriteLn(F, IntToStr(path[i] + 1), ' -> ', IntToStr(path[i + 1] + 1),
               ' [label = "', IntToStr(matrix[path[i], path[i + 1]]), '" ',
               ' color = "red"', ' dir = forward',
               ' arrowhead = "open"', ']');

  WriteLn(F, '}');
  CloseFile(F);
end;

//Algorithm finds all possible shortest pathes
//For perfect correctness, matrix should
//have zeros in main diagonal (but it is'n necessary)
//In this implementation, algorithm requires
//n^2 memory for computation
//and his complexity is O(n^3)
//------------------------------------------------------------------------
Var
  matrix        : TTIntArray;
  precendenceMat: TTIntArray;
  start, dest   : Integer;
  path          : TIntArray;
Begin
  Writeln('Floyd-Warshall algorithm');
  //Initialization
  Init(matrix);
  InitPrecendenceMat(precendenceMat, matrix);

  Floyd_Warshall(matrix, precendenceMat);
  // After that, matrix is our solution
  // It represents costs of pathes
  // for every pair of nodes (all costs of all possible routes)
  PrintMatrix(matrix);

  Writeln('Enter start and destination nodes');
  Readln(start, dest);
  start := start - 1;
  dest := dest - 1;
  ReconstructPath(start, dest, matrix, precendenceMat, path);

  Write('Path is : ');
  Write(start + 1);
  PrintPath(path);
  Write(dest + 1);
  WriteLn('');
  WriteLn('------------');
  Write('Cost is ');
  Writeln(matrix[start, dest]);

  //We need our initial graph-matrix
  Init(matrix);
  WriteGraph('output.txt', start, dest, matrix, path);

  Readln(start);
End.
