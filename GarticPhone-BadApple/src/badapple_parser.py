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

    video_properties = get_video_properties(cap)
    frame_drawer = FrameDrawer(video_properties)

    print(video_properties["fps"])

    while cap.isOpened():
        ret, frame = cap.read()

        if not ret:
            break

        frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        frame_drawer.draw_frame(frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()

def get_video_properties(cap: cv2.VideoCapture) -> dict:
    if not cap.isOpened():
        sys.exit("Can Not Get Video Properties If Video Is Not Open")

    video_properties = {}

    video_properties["width"] = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
    video_properties["height"] = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
    video_properties["fps"] = int(cap.get(cv2.CAP_PROP_FPS))
    video_properties["frame_count"] = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))

    return video_properties

    pass