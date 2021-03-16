import { configureStore } from '@reduxjs/toolkit';
import { motivatorSlice } from './motivator/motivatorSlice';
import { sessionSlice } from './session/sessionSlice';

export const store = configureStore({
  reducer: {
    motivator: motivatorSlice.reducer,
    session: sessionSlice.reducer,
  },
  devTools: {
    trace: true,
    shouldCatchErrors: true,
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
