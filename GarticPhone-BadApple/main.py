import sys

from src.badapple_parser import start

def main():
    user_input = get_user_input()

    start(user_input)

def get_user_input() -> dict:
    user_input = {}

    video_path = input("Input Video Path: ")

    user_input["video_path"] = video_path

    try:
        width = int(input("Input Width: "))

        height = int(input("Input Height: "))
    except ValueError:
        sys.exit("Must Be An Integer")
    else:
        user_input["width"] = width
        user_input["height"] = height

    try:
        start_x = int(input("Start X Position: "))

        start_y = int(input("Start Y Position: "))
    except ValueError:
        sys.exit("Must Be An Integer")
    else:
        user_input["start_x"] = start_x
        user_input["start_y"] = start_y

    return user_input

if __name__ == "__main__":
    main()