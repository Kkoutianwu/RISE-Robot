# --**-- coding: utf-8 --**--
# --**-- recognization online based on SSD7 --**--

from keras.optimizers import Adam
from keras.callbacks import ModelCheckpoint, EarlyStopping, ReduceLROnPlateau, TerminateOnNaN, CSVLogger
from keras import backend as K
from keras.models import load_model
from math import ceil
import numpy as np
#from matplotlib import pyplot as plt
import time
import cv2
from models.keras_ssd7 import build_model
from keras_loss_function.keras_ssd_loss import SSDLoss
from keras_layers.keras_layer_AnchorBoxes import AnchorBoxes
from keras_layers.keras_layer_DecodeDetections import DecodeDetections
from keras_layers.keras_layer_DecodeDetectionsFast import DecodeDetectionsFast
from ssd_encoder_decoder.ssd_input_encoder import SSDInputEncoder
from ssd_encoder_decoder.ssd_output_decoder import decode_detections, decode_detections_fast
from data_generator.object_detection_2d_data_generator import DataGenerator
from data_generator.object_detection_2d_misc_utils import apply_inverse_transforms
from data_generator.data_augmentation_chain_variable_input_size import DataAugmentationVariableInputSize
from data_generator.data_augmentation_chain_constant_input_size import DataAugmentationConstantInputSize
from data_generator.data_augmentation_chain_original_ssd import SSDDataAugmentation
import pyrealsense2 as rs



class Online_rec:
    def __init__(self):
        self.img_height =270 # Height of the input images
        self.img_width = 270 # Width of the input images
        self.img_channels = 3 # Number of color channels of the input images
        self.intensity_mean = 127.5 # Set this to your preference (maybe `None`). The current settings transform the input pixel values to the interval `[-1,1]`.
        self.intensity_range = 127.5 # Set this to your preference (maybe `None`). The current settings transform the input pixel values to the interval `[-1,1]`.
        self.n_classes = 12 # Number of positive classes
        self.scales = [0.1, 0.16, 0.32, 0.64, 0.96] # An explicit list of anchor box scaling factors. If this is passed, it will override `min_scale` and `max_scale`.
        self.aspect_ratios = [0.5, 1.0, 1.5] # The list of aspect ratios for the anchor boxes
        self.two_boxes_for_ar1 = False # Whether or not you want to generate two anchor boxes for aspect ratio 1
        self.steps = None # In case you'd like to set the step sizes for the anchor box grids manually; not recommended
        self.offsets = None # In case you'd like to set the offsets for the anchor box grids manually; not recommended
        self.clip_boxes = False # Whether or not to clip the anchor boxes to lie entirely within the image boundaries
        self.variances = [1.0, 1.0, 1.0, 1.0] # The list of variances by which the encoded target coordinates are scaled
        self.normalize_coords = True # Whether or not the model is supposed to use coordinates relative to the image size
        K.clear_session() #清除原有模型
        self.model_path = '0821.h5'
        self.ssd_loss = SSDLoss(neg_pos_ratio=3, alpha=1.0)
        self.model = load_model(self.model_path, custom_objects={'AnchorBoxes': AnchorBoxes,
                                                       'compute_loss': self.ssd_loss.compute_loss})
        self.objtuple = []

    def add_bbox(self, img):  
        img = img
        classes = ['background', 'remote', 'card', 'wallet', 'bottle', 'cup','chess','green','blue', 'phone','cup_green',"cup_blue",'cup']
        colors = [(250, 100, 20), (170, 250, 125), (0, 145, 250), (12, 250, 40), (240, 255, 2),(255,0,150),(0,0,120),(0,0,255),(255,255,255),(0,0,0),(255,255,255),(180,0,180)]
        for box in self.output:
            cls = int(box[0])
            conf = str(box[1])
            xmin = int(box[2]) * 2
            ymin = int(box[3]) * 2
            xmax = int(box[4]) * 2
            ymax = int(box[5]) * 2
            cv2.rectangle(img, (xmin, ymin), (xmax, ymax), colors[cls - 1], 4)
            #cv2.putText(img, classes[cls], (xmin, ymin), cv2.FONT_HERSHEY_COMPLEX, 1, colors[cls - 1], 1)
            centx = (xmin + xmax)//2
            centy = (ymin + ymax)//2
            self.objtuple.append((centx, centy, cls))
        return img
    def onlion_rec(self, img1):  #输入应该是bgr的540×540的三通道图片
        self.objtuple = []
        img1 = cv2.cvtColor(img1, cv2.COLOR_BGR2RGB)
        dst = cv2.resize(img1, (270, 270), cv2.INTER_LINEAR)
        img1 = dst.reshape(1, 270, 270, 3)
        y_pred = self.model.predict(img1)
        y_pred_decoded = decode_detections(y_pred,
                                           confidence_thresh=0.92,
                                           iou_threshold=0.1,
                                           top_k=200,
                                           normalize_coords=self.normalize_coords,
                                           img_height=self.img_height,
                                           img_width=self.img_width)
        #print("Predicted boxes:\n")
        #print('   class   conf xmin   ymin   xmax   ymax')
        #print(y_pred_decoded[0])
        self.output = y_pred_decoded[0]


