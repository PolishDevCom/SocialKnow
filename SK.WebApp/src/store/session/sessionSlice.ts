import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface SessionState {
  username: string;
  token: string;
  image: string;
}

const initialState: SessionState = {
  username: '',
  token: '',
  image: '',
};

export const sessionSlice = createSlice({
  initialState,
  name: 'session',
  reducers: {
    setSession: (state, action: PayloadAction<SessionState>) => {
      if (action?.payload) {
        state.username = action.payload.username;
        state.token = action.payload.token;
        state.image = action.payload.image;
      }
    },
  },
});
