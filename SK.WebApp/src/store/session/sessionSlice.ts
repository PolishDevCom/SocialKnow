import { createSelector, createSlice } from "@reduxjs/toolkit";
import { RootState } from '..';

interface SessionState {
  username: string;
  token: string;
  image: string;
};

const initialState : SessionState = {
  username: "",
  token: "",
  image: "",
};

export const sessionSlice = createSlice({
    initialState,
    name: 'session',
    reducers: {
      saveLoginData: (state, action) => {
        if (action?.payload) {
            state.username = action.payload.username;
            state.token = action.payload.token;
            state.image = action.payload.image;
        }
      }
    },
  });

export const sessionSelector = createSelector<
  RootState,
  SessionState['username'],
  SessionState['token'],
  SessionState['image'],
  SessionState
>(
  (state) => state.session.username,
  (state) => state.session.token,
  (state) => state.session.image,
  (username, token, image) => {
    return {
      username,
      token,
      image,
    };
  },
);