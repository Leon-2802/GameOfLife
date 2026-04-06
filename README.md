# GameOfLife
Conway's Game of Life as you know it.
![gameoflife](https://github.com/Leon-2802/GameOfLife/assets/72872011/0159cc52-2ddc-476c-bd58-2239db6c29d9)

### About
#### What is Conway's Game Of Life?
It is a cellular automaton that plays out on a 2D grid and updates its state in discrete time steps (generations).

The state of the cells on the grid is defined by simple rules:
1. Any live cell with fewer than two live neighbours dies, as if by underpopulation.
2. Any live cell with two or three live neighbours lives on to the next generation.
3. Any live cell with more than three live neighbours dies, as if by overpopulation.
4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

See the [wikipedia article](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life) for more detailed information.
For a further deep-dive into the realm of emergent complexity, I highly recommend watching the following [video](https://www.youtube.com/watch?v=0HqUYpGQIfs).

#### Why did I choose to write my own implementation?
I started this project in 2022, at the beginning of my computer science journey. I mostly followed a tutorial and did not know what I was doing.
In 2026, during my parallel computing class I searched for an old project of mine that I could parallelize and improve the performance of. 
So I came back to this project, and got to work:
1. I refactored the old (really bad) code with the knowledge I had gained in the meantime (See the archive branch if you want to see the code before I started refactoring).
2. Then I added tests and a CI/CD pipeline before changing anything related to the core functionality.
3. I parallelized the advance-step between the generations and the initialization of the grid.
4. With the gained speed-up I followed [Gustafson's law](https://en.wikipedia.org/wiki/Gustafson%27s_law) and made the grid larger. The goal was to get the grid as large as possible while staying below 100ms calculation time per advance-step. The maximum grid size that I could reach while barely staying below the 100ms mark on my system was 1500 x 1500.

Of course the end result does not please a performance oriented developer. I could try to push the performance further and make it more adaptable to different systems while sticking to C# and its concurrency framework, but that is not what I want to do. During my parallel computing class, we wrote a few projects with NVIDIAs CUDA framework. As Game Of Life is perfectly parallel, it fits the SIMD-architecture of a GPU perfectly, which should make it a perfect choice for a (beginner) [CUDA project](https://github.com/Leon-2802/CUDA-Driven-GameOfLife).

### Running the program
Navigate to the Actions-tab of this repo and select the newest workflow run. Then scroll down and download the artifact.
You will recieve a zipped Windows-executable.

Please note that I developed this project on a 12-core, 3.7GHz CPU with 32GB of RAM. If your system has lower specs, the advance-steps between the generations can take a lot longer than the 100ms I aimed for, as the size of the grid is not adaptable. 