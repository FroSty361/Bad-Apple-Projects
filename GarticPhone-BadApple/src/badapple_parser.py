import cv2
import sys
from .draw_frame import FrameDrawer
from pathlib import Path

def start(user_input: dict):
    video_path = Path(user_input["video_path"])

    if not video_path.exists():
        sys.exit(f"Video Path {video_path} Does Not Exist")

    cap = cv2.VideoCapture(video_path)

    if not cap.isOpened():
        sys.exit("Could Not Open Video")

    frame_drawer = FrameDrawer(user_input)

    while cap.isOpened():
        ret, frame = cap.read()

        if not ret:
            break

        frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        frame = cv2.resize(frame, (user_input["width"], user_input["height"]))

        frame_drawer.draw_frame(frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()

    return cap

    pass