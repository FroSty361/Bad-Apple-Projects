import cv2
import pyautogui

class FrameDrawer:
    def __init__(self, video_properties: dict):
        screenwidth, screenheight = pyautogui.size()

        self.screenwidth = screenwidth
        self.screenheight = screenheight

        self.video_properties = video_properties

    def draw_frame(self, frame):
