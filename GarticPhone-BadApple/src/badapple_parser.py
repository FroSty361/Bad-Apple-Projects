import cv2
import sys
import time
from .draw_frame import FrameDrawer
from pathlib import Path

def start(user_input: dict):
    video_path = Path(user_input["video_path"])

    if not video_path.exists():
        sys.exit(f"Video Path {video_path} Does Not Exist")

    cap = cv2.VideoCapture(video_path)

    if not cap.isOpened():
        sys.exit("Could Not Open Video")

    time.sleep(3)

    frame_drawer = FrameDrawer(user_input)

    i = 0

    while cap.isOpened():
        ret, frame = cap.read()

        if not ret:
            break

        frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        frame = cv2.resize(frame, (user_input["width"], user_input["height"]))

        frame_drawer.draw_frame(frame)

        i = i + 1
        print(f"Frame Number = {i}")

    cap.release()