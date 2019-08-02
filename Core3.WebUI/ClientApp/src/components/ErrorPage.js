import React, { Component } from 'react'
import img from '../img/ups.png'

export default class ErrorPage extends Component {
    render() {
        return (
            <img src={img} alt="error message" />
        )
    }
}