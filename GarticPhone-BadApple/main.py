import csv
import sys
from src.badapple_parser import start
from pathlib import Path

def main():
    choice = input("Input CSV {1} Or Input Manual Values {2} ")

    if choice == "1":
        user_input = get_user_input_csv()
    elif choice == "2":
        user_input = get_user_input_manual()
    else:
        sys.exit("Must Input {1}} Or {2}")

    start(user_input)

def get_user_input_csv() -> dict:
    csv_output = {}

    csv_path = Path(input("Input CSV Path: "))

    if not csv_path.exists():
        sys.exit(f"CSV Path {csv_path} Does Not Exist")

    with open(csv_path, mode='r', encoding='utf-8') as csv_file:
        csv_reader = csv.reader(csv_file)

        for row in csv_reader:
            csv_output[row[0]] = row[1]

    user_input = {}

    possible_keys = [ "video_path", "width", "height", "start_x", "start_y" ]

    for key, value in csv_output.items():
        if key not in possible_keys:
            sys.exit(f"Key {key} Does Not Exist")

        if key == "video_path":
            user_input["video_path"] = value
        else: # For Integers
            try:
                user_input[key] = int(value)
            except ValueError:
                sys.exit(f"{key} Value Must Be An Integer")

    return user_input

def get_user_input_manual() -> dict:
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