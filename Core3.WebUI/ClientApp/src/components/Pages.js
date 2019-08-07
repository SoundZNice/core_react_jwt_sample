import { Home } from './components/Home'
import Login from './components/Login'
import Register from './components/Register'
import Notes from './components/Notes'
import ErrorPage from './components/ErrorPage'
import UserInfo from './components/User/UserInfo'
import UserNotes from './components/User/UserNotes'

export const routes = {};

(function(ctx){
    const p = {

        'general': [{
            route: '/',
            component: Home 
        },{
            route: '/500',
            component: ErrorPage
        }],

        'noauth': [,{
            route: '/notes',
            component: Notes,
        },,{
            route: '/login',
            component: Login
        },{
            route: '/register',
            component: Register
        }],

        'auth': [{
            route: '/user-notes',
            component: UserNotes
        },{
            route: '/user-info',
            component: UserInfo
        }]
    }

    const getCore = (state) => [p[state], ...p['general']]

    ctx.get = (state) => {
        if (!state)
            console.error("No state")
        
        if (!p[state])
            console.error("No such state")

        return getCore(state)
    }

})(routes)

