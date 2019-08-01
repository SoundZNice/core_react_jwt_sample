import React, { Component } from 'react'
import { Toast, ToastBody } from 'reactstrap';
import '../styles/styles.css'

export default class Note extends Component {
    constructor (props) {
        super(props);

        this.state = { 
            id: this.props.id, 
            text: this.props.text 
        };
    }

    render() {
        return (
            <Toast className="note">
                <ToastBody>
                    {this.state.text}
                </ToastBody>
            </Toast>
        )
    }
}