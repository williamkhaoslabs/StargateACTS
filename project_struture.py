import os

# ANSI color codes
RESET = "\033[0m"
BLACK = "\033[30m"
RED = "\033[31m"
ORANGE = "\033[38;5;208m"
GREEN = "\033[32m"
BLUE = "\033[34m"
INDIGO = "\033[38;5;54m"
VIOLET = "\033[35m"

FOLDER_COLORS = [RED, ORANGE, GREEN, BLUE, INDIGO, VIOLET]


def get_folder_color(depth: int) -> str:
    """
    depth 0 = root-level child folders -> red
    depth 1 = next child folders -> orange
    ...
    depth 5+ = violet
    """
    if depth < len(FOLDER_COLORS):
        return FOLDER_COLORS[depth]
    return VIOLET


def build_tree(start_path: str, output_file: str = "project_tree.txt"):
    start_path = os.path.abspath(start_path)
    root_name = os.path.basename(start_path)

    with open(output_file, "w", encoding="utf-8") as f:
        # Write project root name in red
        f.write(f"{RED}{root_name}/{RESET}\n")

        for current_root, dirs, files in os.walk(start_path):
            # Optional: skip common junk folders
            dirs[:] = [d for d in dirs if d not in {".git", ".vs", "bin", "obj"}]

            # Keep things sorted
            dirs.sort()
            files.sort()

            rel_path = os.path.relpath(current_root, start_path)
            depth = 0 if rel_path == "." else rel_path.count(os.sep) + 1

            indent = "    " * depth

            # Write child folders
            for d in dirs:
                folder_color = get_folder_color(depth)
                f.write(f"{indent}{folder_color}{d}/{RESET}\n")

            # Write files in black/plain
            for file_name in files:
                f.write(f"{indent}{BLACK}{file_name}{RESET}\n")

    print(f"Created: {output_file}")


if __name__ == "__main__":
    build_tree(".")