import { RouteProps } from 'react-router-dom';
import { IntegrationDemo } from './pages/IntegrationDemo';
import { Home } from './pages/Home';

export const routes: RouteProps[] = [
    { path: '/integrationDemo', component: IntegrationDemo },
    { path: '/', component: Home },
];
