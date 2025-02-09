from collections import deque
import random


def create_shuffled_queue(string_list):
    shuffled_list = string_list.copy()
    random.shuffle(shuffled_list)
    return deque(shuffled_list)