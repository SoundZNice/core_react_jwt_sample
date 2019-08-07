import { Home } from './Home'
import Login from './Login'
import Register from './Register'
import Notes from './Notes'
import ErrorPage from './ErrorPage'
import UserInfo from './User/UserInfo'
import UserNotes from './User/UserNotes'

export const routes = {};

(function(ctx){
    const p = {

        'general': [{
            route: '/',
            component: Home,
            name: 'Home',
            nav: true 
        },{
            route: '/500',
            component: ErrorPage
        }],

        'noauth': [{
            route: '/notes',
            component: Notes,
            name: 'Notes',
            nav: true 
        },{
            route: '/login',
            component: Login,
            name: 'Login',
            nav: true 
        },{
            route: '/register',
            component: Register,
            name: 'Register',
            nav: true 
        }],

        'auth': [{
            route: '/user-notes',
            component: UserNotes,
            name: 'User Notes',
            nav: true 
        },{
            route: '/user-info',
            component: UserInfo,
            name: 'User Info',
            nav: true 
        }]
    }

    const getCore = (state) => [...p['general'], ...p[state]]

    ctx.get = (state) => {
        if (!state)
            console.error("No state")
        
        if (!p[state])
            console.error("No such state")

        return getCore(state)
    }

})(routes)

