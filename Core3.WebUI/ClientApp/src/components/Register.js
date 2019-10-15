import React, { Component } from 'react'
import { Button, Form, FormGroup, Label, Input } from 'reactstrap'

export default class Register extends Component {
    constructor(props){
        super(props);

        this.state = {
            login: '',
            password: '',
            confirmPassword: '',
            displayName: ''
        }
    }

    onLoginChange = (e) => {
        this.setState({login: e.target.value})
    }

    onPasswordChange = (e) => {
        this.setState({password: e.target.value})
    }

    onConfirmPasswordChange = (e) => {
        this.setState({confirmPassword: e.target.value})
    }

    onDisplayNameChange = (e) => {
        this.setState({displayName: e.target.value})
    }

    onSubmit = (e) => {
        console.log(this.state)
        e.preventDefault();
    }

    render () {
        return (
            <Form onSubmit={this.onSubmit}>
                <FormGroup>
                    <Label for="login">Login</Label>
                    <Input type="text" 
                        name="login" 
                        id="login" 
                        placeholder="..." 
                        onChange={this.onLoginChange} />
                </FormGroup>
                <FormGroup>
                    <Label for="password">Password</Label>
                    <Input type="password"
                        name="password"
                        id="password"
                        placeholder="..."
                        onChange={this.onPasswordChange} />
                </FormGroup>
                <FormGroup>
                    <Label for="confirm-password">Confirm Password</Label>
                    <Input type="password"
                        name="confirm-password"
                        id="confirm-password"
                        placeholder="..."
                        onChange={this.onConfirmPasswordChange} />
                </FormGroup>
                <FormGroup>
                    <Label for="display-name">Display Name</Label>
                    <Input type="text"
                        name="display-name"
                        id="display-name"
                        placeholder="..."
                        onChange={this.onDisplayNameChange} />
                </FormGroup>
                <Button>Submit</Button>
            </Form>
        )
    }
}
