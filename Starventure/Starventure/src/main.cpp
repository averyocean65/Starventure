#include <raylib.h>

int main(int argc, char** argv) {
	int height = 960;
	int width = 540;

	InitWindow(width, height, "Starventure");
	SetTargetFPS(60);

	while (!WindowShouldClose()) {
		BeginDrawing();

		ClearBackground(BLACK);
		DrawText("Starventure will now be coming to Raylib and C++!", width/2, height/2, 12, WHITE);

		EndDrawing();
	}

	CloseWindow();

	return 0;
}